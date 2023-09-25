using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace OccaSoftware.MotionBlur.Runtime
{
    public class MotionBlurRendererFeature : ScriptableRendererFeature
    {
        class MotionBlurRenderPass : ScriptableRenderPass
        {
            private const string shaderName = "MotionBlurCompute";
            private ComputeShader shader = null;

            private RTHandle motionBlurTarget;
            private RTHandle source;

            private const string motionBlurTargetId = "_MotionBlurTarget";
            private const string kernelId = "ComputeMotionBlur";

            private const string profilerTag = "Motion Blur";
            private const string cmdBufferName = "Motion Blur";

            Material motionVectors;

            int targetKernel;
            int groupsX;
            int groupsY;

            public MotionBlurRenderPass()
            {
                motionBlurTarget = RTHandles.Alloc(Shader.PropertyToID(motionBlurTargetId), name: motionBlurTargetId);
            }

            BetterMotionBlur motionBlur;

            public void Dispose()
            {
                motionBlurTarget?.Release();
                motionBlurTarget = null;

                CoreUtils.Destroy(motionVectors);
                motionVectors = null;

                shader = null;
            }

            public void SetTarget(RTHandle source)
            {
                this.source = source;
            }

            public void Setup(BetterMotionBlur motionBlur)
            {
                this.motionBlur = motionBlur;

                if (motionVectors == null)
                {
                    motionVectors = CoreUtils.CreateEngineMaterial(Shader.Find("Hidden/Universal Render Pipeline/CameraMotionVectors"));
                }
            }

            /// <summary>
            /// Loads the compute shader for Motion Blur.
            /// </summary>
            /// <returns>True if the shader was successfully loaded, false otherwise.</returns>
            public bool LoadComputeShader()
            {
                if (shader != null)
                    return true;

                shader = (ComputeShader)Resources.Load(shaderName);
                if (shader == null)
                    return false;

                return true;
            }

            private int GetGroupCount(int textureDimension, uint groupSize)
            {
                return Mathf.CeilToInt((textureDimension + groupSize - 1) / groupSize);
            }

            public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
            {
                ConfigureTarget(source);

                RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;
                descriptor.depthBufferBits = 0;
                descriptor.enableRandomWrite = true;
                descriptor.msaaSamples = 1;
                descriptor.sRGB = false;

                descriptor.width = Mathf.Max(1, descriptor.width);
                descriptor.height = Mathf.Max(1, descriptor.height);

                RenderingUtils.ReAllocateIfNeeded(
                    ref motionBlurTarget,
                    descriptor,
                    FilterMode.Point,
                    TextureWrapMode.Clamp,
                    name: motionBlurTargetId
                );

                targetKernel = shader.FindKernel(kernelId);

                shader.GetKernelThreadGroupSizes(targetKernel, out uint threadGroupSizeX, out uint threadGroupSizeY, out _);
                groupsX = GetGroupCount(descriptor.width, threadGroupSizeX);
                groupsY = GetGroupCount(descriptor.height, threadGroupSizeY);
            }

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                Profiler.BeginSample(profilerTag);
                CommandBuffer cmd = CommandBufferPool.Get(cmdBufferName);

                cmd.SetComputeTextureParam(shader, targetKernel, ShaderParams._ScreenTexture, source);
                cmd.SetComputeTextureParam(shader, targetKernel, ShaderParams._MotionBlurTarget, motionBlurTarget);

                cmd.SetComputeVectorParam(
                    shader,
                    ShaderParams._ScreenSizePx,
                    new Vector2(renderingData.cameraData.cameraTargetDescriptor.width, renderingData.cameraData.cameraTargetDescriptor.height)
                );
                float currentFramerate = 1.0f / Time.unscaledDeltaTime;
                float shutterSpeed = motionBlur.shutterSpeed.GetValue<float>();
                float velocityScale = currentFramerate * shutterSpeed;
                float depthSeparationInvDistance = 1.0f / motionBlur.depthSeparationDistance.GetValue<float>();
                cmd.SetComputeIntParam(shader, ShaderParams._DepthSeparationEnabled, (int)motionBlur.depthSeparationMode.value);
                cmd.SetComputeFloatParam(shader, ShaderParams._DepthSeparationInverseDistance, depthSeparationInvDistance);
                cmd.SetComputeIntParam(shader, ShaderParams._FrameId, Time.frameCount);
                cmd.SetComputeFloatParam(shader, ShaderParams._VelocityScale, velocityScale);
                cmd.SetComputeIntParam(shader, ShaderParams._MaxSamples, motionBlur.QualityValue(motionBlur.qualityLevel.value));

                // Make sure that the Motion Vector Texture is initialized by Unity
                if (Shader.GetGlobalTexture("_MotionVectorTexture") != null)
                {
                    cmd.DispatchCompute(shader, targetKernel, groupsX, groupsY, 1);
                    Blitter.BlitCameraTexture(cmd, motionBlurTarget, source);
                }

                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();
                CommandBufferPool.Release(cmd);
                Profiler.EndSample();
            }

            private static class ShaderParams
            {
                public static int _ScreenTexture = Shader.PropertyToID("_ScreenTexture");
                public static int _ScreenSizePx = Shader.PropertyToID("_ScreenSizePx");
                public static int _DepthSeparationEnabled = Shader.PropertyToID("_DepthSeparationEnabled");
                public static int _DepthSeparationInverseDistance = Shader.PropertyToID("_DepthSeparationInverseDistance");
                public static int _VelocityScale = Shader.PropertyToID("_VelocityScale");
                public static int _MaxSamples = Shader.PropertyToID("_MaxSamples");
                public static int _FrameId = Shader.PropertyToID("_FrameId");
                public static int _MotionBlurTarget = Shader.PropertyToID("_MotionBlurTarget");
            }

            public override void OnCameraCleanup(CommandBuffer cmd) { }
        }

        [System.Serializable]
        public class Settings
        {
            public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingTransparents;
        }

        public Settings settings = new Settings();
        MotionBlurRenderPass motionBlurPass = null;

        public override void Create()
        {
            Dispose();

            motionBlurPass = new MotionBlurRenderPass();
            motionBlurPass.renderPassEvent = settings.renderPassEvent;
        }

        private bool DeviceSupportsComputeShaders()
        {
            const int _COMPUTE_SHADER_LEVEL = 45;
            if (SystemInfo.graphicsShaderLevel >= _COMPUTE_SHADER_LEVEL)
                return true;

            return false;
        }

        BetterMotionBlur motionBlur = null;

        /// <summary>
        /// Get the Auto Exposure component from the Volume Manager stack.
        /// </summary>
        /// <returns>If Auto Exposure component is null or inactive, returns false.</returns>
        internal bool RegisterMotionBlurStackComponent()
        {
            motionBlur = VolumeManager.instance.stack.GetComponent<BetterMotionBlur>();
            if (motionBlur == null)
                return false;

            return motionBlur.IsActive();
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            bool isActive = RegisterMotionBlurStackComponent();
            if (!isActive)
                return;

            if (!renderingData.cameraData.camera.GetUniversalAdditionalCameraData().renderPostProcessing)
                return;

            if (!DeviceSupportsComputeShaders())
            {
                Debug.LogWarning("Motion Blur requires Compute Shader support.", this);
                return;
            }

            if (IsExcludedCameraType(renderingData.cameraData.camera.cameraType))
                return;

            if (!motionBlurPass.LoadComputeShader())
                return;

            renderer.EnqueuePass(motionBlurPass);
        }

        public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
        {
            motionBlurPass.Setup(motionBlur);
            motionBlurPass.ConfigureInput(
                ScriptableRenderPassInput.Color
                    | ScriptableRenderPassInput.Depth
                    | ScriptableRenderPassInput.Normal
                    | ScriptableRenderPassInput.Motion
            );
            motionBlurPass.SetTarget(renderer.cameraColorTargetHandle);
        }

        protected override void Dispose(bool disposing)
        {
            motionBlurPass?.Dispose();
            motionBlurPass = null;
            base.Dispose(disposing);
        }

        private bool IsExcludedCameraType(CameraType type)
        {
            switch (type)
            {
                case CameraType.SceneView:
                    return true;
                case CameraType.Preview:
                    return true;
                case CameraType.Reflection:
                    return true;
                default:
                    return false;
            }
        }
    }
}
