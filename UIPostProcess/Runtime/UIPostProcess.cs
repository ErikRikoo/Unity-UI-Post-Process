using UI.PostProcess.Scripts.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UIPostProcess.Runtime
{
    public class UIPostProcess : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Color m_BackgroundColor;
        [Tooltip("This property is mainly to configure the ratio of your screen")]
        [SerializeField] private Vector2 m_Dimension;
        [Tooltip("It will be used by the Render Texture")]
        [SerializeField] private int m_PixelDensity;
        [Tooltip("Will be multiplied with m_Dimension to give the size in units")]
        [SerializeField] private float m_Scale;
        [SerializeField] private Material m_PostProcessMaterial;
    

        [Header("Components")]
        [SerializeField] private Camera m_RenderingCamera;
        [SerializeField] private Canvas m_DrawingCanvas;
        [SerializeField] private Image m_DrawingBackground;
        [SerializeField] private Renderer m_DisplayRenderer;
        [SerializeField] private RectTransform m_UIPlaceHolder;
    

        private RenderTexture m_Buffer;
    
        private static readonly int MainTexShaderID = Shader.PropertyToID("_MainTex");

        private Vector2Int TextureSize => new Vector2Int(
            (int) (m_Dimension.x * m_PixelDensity),
            (int) (m_Dimension.y * m_PixelDensity)
        );
    
        public float Ratio => m_Dimension.x / m_Dimension.y;
    
        private void OnValidate()
        {
            ApplyProperties();
        }

        public void ApplyProperties()
        {
            UpdateMaterial();
            UpdateBackgroundColors();
            UpdateSize();
        }

        private void UpdateMaterial()
        {
            if (m_DisplayRenderer != null)
            {
                m_DisplayRenderer.material = m_PostProcessMaterial;
            }
        }

        private void UpdateSize()
        {
            UpdateCamera();
            UpdateCanvas();
            UpdateDisplayRenderer();
            UpdateRenderTexture();
        }

        private void UpdateDisplayRenderer()
        {
            if (m_DisplayRenderer == null)
            {
                return;
            }
        
            Vector2 newSize = ComputeDimension();
            m_DisplayRenderer.transform.localScale = new Vector3(newSize.x, 1f, newSize.y);
        }

        private void UpdateCanvas()
        {
            if (m_DrawingCanvas == null)
            {
                return;
            }
        
            RectTransform canvasTransform = m_DrawingCanvas.transform as RectTransform;
            Vector2 newSize = ComputeDimension();
            canvasTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newSize.x);
            canvasTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newSize.y);
        }

        private void UpdateCamera()
        {
            if (m_RenderingCamera == null)
            {
                return;
            }
        
            float ratio = Ratio;
            m_RenderingCamera.aspect = ratio;
            m_RenderingCamera.orthographicSize = ComputeDimension().y * 0.5f;
        }

        private void UpdateRenderTexture()
        {
            if (m_RenderingCamera == null || m_DisplayRenderer == null)
            {
                return;
            }
        
            m_RenderingCamera.targetTexture = null;
            if (m_Buffer == null)
            {
                CreateRenderTexture();
            }
            else
            {
                m_Buffer.Release();
                CreateRenderTexture();
            }
            m_RenderingCamera.targetTexture = m_Buffer;
            m_DisplayRenderer.sharedMaterial?.SetTexture(MainTexShaderID, m_Buffer);
        }

        private void CreateRenderTexture()
        {
            Vector2Int textureSize = TextureSize;
            if (textureSize.x == 0 || textureSize.y == 0)
            {
                return;
            }
            m_Buffer = new RenderTexture(textureSize.x, textureSize.y, 24);
            m_Buffer.Create();
        }

        private void UpdateBackgroundColors()
        {
            if (m_RenderingCamera != null)
            {
                m_RenderingCamera.backgroundColor = m_BackgroundColor;
            }

            if (m_DrawingBackground != null)
            {
                m_DrawingBackground.color = m_BackgroundColor;
            }
        }

        private Vector2 ComputeDimension()
        {
            return m_Dimension * m_Scale;
        }
    
#if UNITY_EDITOR
        [HideInInspector]
        [SerializeField] public bool m_EditorToolsFoldoutOpened;
        
        [HideInInspector]
        [SerializeField] public RectTransform m_CanvasElementToMove;

        [HideInInspector]
        [SerializeField] public bool m_SnapElementAnchorsToggleValue;

        public void MakeSelectedObjectCanvasChild()
        {
            Undo.RecordObject(m_CanvasElementToMove, "Move object");

            if (m_SnapElementAnchorsToggleValue)
            {
                m_CanvasElementToMove.SnapHierarchyAnchors();
            }
        
            m_CanvasElementToMove.SetParent(m_UIPlaceHolder, false);
            m_CanvasElementToMove = null;
        }
    
    
        public bool CheckObjectToMoveValidity()
        {
            bool belongsToTheScene = false;
            foreach (var root in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                if (IsCanvasElementToMoveChildOf(root.transform))
                {
                    belongsToTheScene = true;
                    break;
                }
            }

            if (!belongsToTheScene)
            {
                return false;
            }
        
        
            if (IsCanvasElementToMoveChildOf(transform))
            {
                return false;
            }

            return true;
        }

        public bool IsCanvasElementToMoveChildOf(Transform _transform)
        {
            return m_CanvasElementToMove.IsChildOf(_transform);
        }
    
#endif
    }
}
