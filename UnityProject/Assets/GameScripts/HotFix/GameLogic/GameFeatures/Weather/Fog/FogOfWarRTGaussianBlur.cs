using UnityEngine;

namespace Logic.Modules.LargeMap.Model.CityMist
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [UnityEngine.Scripting.Preserve]
    public class FogOfWarRTGaussianBlur : MonoBehaviour
    {
        public Shader Shader;
        public Material Material
        {
            get
            {
                return CheckAndCreateMaterial();
            }
        }
        private Material _material;
        
        /// <summary>
        /// 检查和创建Material
        /// </summary>
        /// <returns></returns>
        protected Material CheckAndCreateMaterial()
        {
            if (!Shader || !Shader.isSupported)
            {
                return null;
            }

            if (_material && _material.shader == Shader)
            {
                return _material;
            }

            _material = new Material(Shader);
            _material.hideFlags = HideFlags.DontSave;
            return _material;
        }
        
        /// <summary>
        /// 迭代此时
        /// </summary>
        [Range(0, 4)]
        public int Iterations = 1;

        /// <summary>
        /// 模糊扩散
        /// </summary>
        [Range(0.2f, 3)]
        public float BlurSpread = 1;

        /// <summary>
        /// 缩放系数
        /// </summary>
        [Range(1, 8)]
        public int DownSample = 1;

        public void Process(RenderTexture src)
        {
            if (Material)
            {
                var rtW = src.width / DownSample;
                var rtH = src.height / DownSample;
                
                // 定义了第一个缓存 bufferO, 并把 src 中的图像缩放后存储到 bufferO 中
                var buffer0 = RenderTexture.GetTemporary(rtW, rtH, 0, src.format);
                Graphics.Blit(src, buffer0);

                for (var i = 0; i < Iterations; i++)
                {
                    Material.SetFloat("_BlurSize", 1 + (i * BlurSpread));
                    
                    // 执行第一个Pass
                    var buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0, src.format);
                    Graphics.Blit(buffer0, buffer1, Material, 0);
                    RenderTexture.ReleaseTemporary(buffer0);

                    // 调用第二个Pass
                    buffer0 = RenderTexture.GetTemporary(rtW, rtH, 0, src.format);
                    Graphics.Blit(buffer1, buffer0, Material, 1);
                    RenderTexture.ReleaseTemporary(buffer1);
                }

                // 迭代完成后 bufferO 将存储最终的图像
                Graphics.Blit(buffer0, src);
                // 释放缓存
                RenderTexture.ReleaseTemporary(buffer0);
            }
        }
        
        // // 第一个版本，最简单的OnRenderlmage实现：
        // private void OnRenderImage(RenderTexture src, RenderTexture dest)
        // {
        //     if (Material != null)
        //     {
        //         int rtW = src.width;
        //         int rtH = src.height;        
        //         // 利用 RenderTexture GetTemporary 函数分配了一块与屏幕图像大小相同的缓冲区。
        //         // 这是因为 高斯模糊需要调用两个Pass, 我们需要使用一块中间缓存来存储第一个Pass执行完毕后得到的模糊结果。
        //         RenderTexture buffer = RenderTexture.GetTemporary(rtW, rtH, 0);

        //         // 使用 Shader 中的第1个Pass （即使用竖直方向的一维高斯核进行滤波）对src进行处理，并将结果存储在了buffer中。
        //         Graphics.Blit(src, buffer, Material, 0);

        //         // 然后，再使用Shader中的第2个Pass (即使用水平方向的一维高斯核进行滤波）对buffer进行处理，返回最终屏幕图像。
        //         Graphics.Blit(buffer, dest, Material, 1);

        //         // 最后 还需要释放之前分配的缓存。
        //         RenderTexture.ReleaseTemporary(buffer);
        //     }
        //     else
        //     {
        //         Graphics.Blit(src, dest);
        //     }
        // }

        // // 第二个版本实现：利用缩放对图像进行降采样，从而减少需要处理的像素 提高性能。
        // private void OnRenderImage(RenderTexture src, RenderTexture dest)
        // {
        //     if (Material)
        //     {
        //         int rtw = src.width / DownSample;
        //         int rtH = src.height / DownSample;
        //         // 使用了小于原屏幕分辨率的尺寸
        //         RenderTexture buffer = RenderTexture.GetTemporary(rtw, rtH, 0);
        //         // 并将该临时渲染纹理的滤波模式设置为双线性
        //         buffer.filterMode = FilterMode.Bilinear;
        //         // 这样，在调用第一个 Pass 时，我们需要处理的像素个数就是原来的几分之一。
        //         // 对图像进行降采样不仅可以减少需要处理的像素个数，提高性能，而且适当的降采样往往还可以得到更好的模糊效果。
        //         // 尽管 downSample 值越大，性能越好，但过大的 downSample 可能会造成图像像素化。

        //         Graphics.Blit(src, buffer, Material, 0);

        //         Graphics.Blit(buffer, dest, Material, 1);

        //         RenderTexture.ReleaseTemporary(buffer);
        //     }
        //     else
        //     {
        //         Graphics.Blit(src, dest);
        //     }
        // }

        // // 第三个版本实现：考虑了高斯模糊的迭代次数，何利用两个临时缓存在迭代之间进行交替的过程。
        // private void OnRenderImage(RenderTexture src, RenderTexture dest)
        // {
        //     if (Material)
        //     {
        //         int rtW = src.width / DownSample;
        //         int rtH = src.height / DownSample;
        //
        //         // 定义了第一个缓存 bufferO, 并把 src 中的图像缩放后存储到 bufferO 中
        //         RenderTexture buffer0 = RenderTexture.GetTemporary(rtW, rtH, 0);
        //         buffer0.filterMode = FilterMode.Bilinear;
        //
        //         Graphics.Blit(src, buffer0);
        //
        //         for (var i = 0; i < Iterations; i++)
        //         {
        //             Material.SetFloat("_BlurSize", 1 + i * BlurSpread);
        //             // 定义了第二个缓存 bufferl
        //             RenderTexture buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0);
        //
        //             // 执行第一个 Pass 时，输入 bufferO, 输出是 bufferl
        //             Graphics.Blit(buffer0, buffer1, Material, 0);
        //
        //             // 完毕后首先把 bufferO 释放，再把结果值 buffer 存储到 bufferO 中，重新分配 bufferl
        //             RenderTexture.ReleaseTemporary(buffer0);
        //             buffer0 = buffer1;
        //             buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0);
        //
        //             // 调用第二个Pass, 重复上述过程
        //             Graphics.Blit(buffer0, buffer1, Material, 1);
        //
        //             // 完毕后再把 bufferO 释放，再把结果值 buffer 存储到 bufferO 中
        //             RenderTexture.ReleaseTemporary(buffer0);
        //             buffer0 = buffer1;
        //         }
        //
        //         // 迭代完成后 bufferO 将存储最终的图像，将结果显示到屏幕上
        //         Graphics.Blit(buffer0, dest);
        //         // 释放缓存
        //         RenderTexture.ReleaseTemporary(buffer0);
        //     }
        //     else
        //     {
        //         Graphics.Blit(src, dest);
        //     }
        // }
    }
}
