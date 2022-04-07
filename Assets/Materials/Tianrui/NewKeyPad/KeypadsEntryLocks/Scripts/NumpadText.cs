using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BigBlit.ActivePack;


namespace BigBlit.Keypads
{

    public interface INumpadText
    {
        string Text { get; set; }
        Color TextColor { get; set; }
        Font TextFont { get; set; }
        FontStyle TextFontStyle { get; set; }
    }

    public class NumpadText : MaskableGraphic, INumpadText
    {

        #region FIELDS AND PROPERTIES

        public string Text {
            get => _text;
            set {
                if (string.IsNullOrEmpty(value)) {
                    if (string.IsNullOrEmpty(_text))
                        return;

                    _text = "";
                }
                else {
                    _text = value;
                }

                requestFontRebuild();
                SetVerticesDirty();
                SetLayoutDirty();
            }
        }

        public Color TextColor {
            get => color;
            set {
                if (color == value)
                    return;
                color = value;
            }
        }


        public int CellsNum {
            get {
                if (_autoCellsNum)
                    return _text.Length;
                return _cellsNum;
            }
            set {
                if (_cellsNum == value)
                    return;

                _cellsNum = value;

                requestFontRebuild();
                SetVerticesDirty();
                SetLayoutDirty();
            }
        }

        public FontStyle TextFontStyle {
            get => _textFontStyle;
            set {
                if (_textFontStyle == value)
                    return;

                _textFontStyle = value;

                requestFontRebuild();
                SetMaterialDirty();
                SetVerticesDirty();
            }
        }
         


        public Font TextFont {
            get => _textFont;
            set {
                if (_textFont == value)
                    return;

                if (isActiveAndEnabled)
                    unregisterFontRebuild(_textFont, onTextureRebuild);

                _textFont = value;

                if (isActiveAndEnabled)
                    registerFontRebuild(_textFont, onTextureRebuild);

#if UNITY_EDITOR
                _registeredFont = _textFont;
#endif

                requestFontRebuild();
                SetMaterialDirty();
                SetVerticesDirty();

            }
        }


        public virtual Material defaultFontMaterial {
            get {
                if (_defaultMaterial == null) {
                    _defaultMaterial = new Material(Shader.Find("Hidden/UI/NumpadText"));
                    _defaultMaterial.hideFlags = HideFlags.HideAndDontSave;
                    _defaultMaterial.name = "Keypad Material";
                }
                return _defaultMaterial;
            }
        }

        public override Material defaultMaterial {
            get {
                return defaultFontMaterial;
            }
        }

        public override Texture mainTexture {
            get {


                if (_textFont != null && _textFont.material != null && _textFont.material.mainTexture != null)
                    return _textFont.material.mainTexture;

                if (m_Material != null)
                    return m_Material.mainTexture;

                return base.mainTexture;
            }
        }


        private static Dictionary<Font, HashSet<Action>> _rebuildFontsCallbacks = new Dictionary<Font, HashSet<Action>>();

        private static Material _defaultMaterial = null;


        [SerializeField] private bool _autoCellsNum;

        [SerializeField] private string _text;

        [SerializeField] private Font _textFont;

        [SerializeField] private FontStyle _textFontStyle;

        [SerializeField] private int _cellsNum;

        [Range(0.0f, 1.0f)]
        [SerializeField] private float _spacing;

        [SerializeField] private int _fontSize;

        [SerializeField] public bool _charStretching;

        [NonSerialized] private bool _isMeshUpdating;

#if UNITY_EDITOR
        private Font _registeredFont;
#endif

        #endregion

        #region uGUI Methods

        protected NumpadText() {
            useLegacyMeshGeneration = false;
        }

        protected override void OnEnable() {
            base.OnEnable();

            registerFontRebuild(_textFont, onTextureRebuild);
            requestFontRebuild();

        }

        protected override void OnDisable() {

            unregisterFontRebuild(_textFont, onTextureRebuild);
            base.OnDisable();
        }


        protected override void OnPopulateMesh(VertexHelper vh) {
            generateTextMesh(vh);
        }

        #endregion

        #region UNITY METHODS

#if UNITY_EDITOR
        protected override void OnValidate() {
            if (!IsActive()) {
                base.OnValidate();
                return;
            }

            if (_textFont != _registeredFont) {
                if (isActiveAndEnabled) {
                    unregisterFontRebuild(_registeredFont, onTextureRebuild);
                    registerFontRebuild(_textFont, onTextureRebuild);
                    _textFont.RequestCharactersInTexture(_text, _fontSize, _textFontStyle);
                }
                _registeredFont = _textFont;
            }

            requestFontRebuild();

            base.OnValidate();

        }

        public override void OnRebuildRequested() {

            if (isActiveAndEnabled) {
                requestFontRebuild();
            }

            base.OnRebuildRequested();
        }
#endif
        #endregion

        #region PRIVATE METHODS

        #region FONT REBUILD
        private static void registerFontRebuild(Font font, Action onFontRebuild) {
            if (font == null)
                return;

            if (!_rebuildFontsCallbacks.TryGetValue(font, out var rebuildCallbacks)) {
                if (_rebuildFontsCallbacks.Count == 0) {
                    Font.textureRebuilt += onfontTextureRebuild;
                }

                rebuildCallbacks = new HashSet<Action>();
                _rebuildFontsCallbacks.Add(font, rebuildCallbacks);
            }

            rebuildCallbacks.Add(onFontRebuild);

        }

        private static void unregisterFontRebuild(Font font, Action onFontRebuild) {
            if (font == null)
                return;

            if (!_rebuildFontsCallbacks.TryGetValue(font, out var rebuildCallbacks))
                return;

            if (rebuildCallbacks.Remove(onFontRebuild)) {
                if (rebuildCallbacks.Count == 0) {
                    _rebuildFontsCallbacks.Remove(font);
                    if (_rebuildFontsCallbacks.Count == 0)
                        Font.textureRebuilt -= onfontTextureRebuild;
                }
            }
        }

        private static void onfontTextureRebuild(Font font) {
            if (!_rebuildFontsCallbacks.TryGetValue(font, out var rebuildCallbacks))
                return;

            foreach (var rebuildCallback in rebuildCallbacks)
                rebuildCallback?.Invoke();
        }


        private void onTextureRebuild() {

            if (this == null || !IsActive() || _isMeshUpdating) {
                UpdateMaterial();
                return;
            }

            requestFontRebuild();

            if (CanvasUpdateRegistry.IsRebuildingGraphics() || CanvasUpdateRegistry.IsRebuildingLayout()) {
                UpdateGeometry();
                UpdateMaterial();
            }
            else {

                SetAllDirty();
            }
        }

        #endregion

        #region MESH GENERATION

        private void generateTextMesh(VertexHelper vh) {

            _isMeshUpdating = true;

            if (_textFont == null) {

                vh.Clear();
                _isMeshUpdating = false;
                return;
            }

            vh.Clear();

            int cellsNum = CellsNum;
            Rect keypadRect = GetPixelAdjustedRect();
            float spacing = (keypadRect.width / cellsNum) * _spacing; //spacing in world coordinates
            float totalSpacing = spacing * (cellsNum - 1);
            float cellWidth = (keypadRect.width - totalSpacing) / (float)cellsNum;
            float cellHeight = keypadRect.height;
            int realCellsNum = Mathf.Min(cellsNum, _text.Length);
            Vector2 cellSize = new Vector2(cellWidth, cellHeight);
            Rect cellRect = new Rect(new Vector2(keypadRect.xMin, keypadRect.yMin), cellSize);

            CharacterInfo ci;
            Vector2 charSize = Vector2.zero;


            for (int i = 0; i < realCellsNum; i++) {
                if (_textFont.GetCharacterInfo(_text[i], out ci, _fontSize, _textFontStyle)) {          
                    charSize.x = (float)(ci.maxX - ci.minX);
                    charSize.y = (float)(ci.maxY - ci.minY);
                    float xx = ((float)ci.minX / (float)ci.maxX) * 0.5f;
                    float yy = ((float)ci.minY / (float)ci.maxY) * 0.5f;

                    float charRatio = (float) ci.maxY / (float)ci.maxX;
                    float cellRatio = cellSize.y / cellSize.x;
                    float xs = cellSize.x;
                    float ys = cellSize.y;

                    if (!_charStretching) {
                        if (charRatio < 1.0f) {
                            ys = cellSize.x * charRatio;
                        }
                        else {
                            xs = cellSize.y * (1.0f / charRatio);
                        }

                        xs = Mathf.Min(cellSize.x, xs);
                        ys = Mathf.Min(cellSize.y, ys);

                    }

                    addCell(vh, new Rect(cellRect.x + xs * xx , cellRect.y + ys * yy, xs-(xx*xs), ys-(yy*ys)), ci.uvBottomLeft, ci.uvTopLeft, ci.uvTopRight, ci.uvBottomRight);
           
                } else {
                    addCell(vh, cellRect, Vector2.zero, Vector2.zero, Vector2.zero, Vector2.zero);
                }
                cellRect.x += cellSize.x + spacing;
            }

            _isMeshUpdating = false;

        }



        private void addCell(VertexHelper vh, Rect cellRect, Vector2 uvBottomLeft, Vector2 uvTopLeft, Vector2 uvTopRight, Vector2 uvBottomRight) {

            addQuad(vh, new Vector3[] {
                new Vector3(cellRect.xMin, cellRect.yMin, 0.0f),
                new Vector3(cellRect.xMin, cellRect.yMax, 0.0f),
                new Vector3(cellRect.xMax, cellRect.yMax, 0.0f),
                new Vector3(cellRect.xMax, cellRect.yMin, 0.0f)},
            new Vector2[] {
                uvBottomLeft,
                uvTopLeft,
                uvTopRight,
                uvBottomRight
                });

        }


        private void addQuad(VertexHelper vh, Vector3[] vertices, Vector2[] uvs) {
            int i = vh.currentVertCount;



            vh.AddVert(vertices[0], color, uvs[0]);
            vh.AddVert(vertices[1], color, uvs[1]);
            vh.AddVert(vertices[2], color, uvs[2]);
            vh.AddVert(vertices[3], color, uvs[3]);

            vh.AddTriangle(i + 0, i + 1, i + 2);
            vh.AddTriangle(i + 2, i + 3, i + 0);


        }
        #endregion


        private void requestFontRebuild() {
            if (_textFont == null && !!_textFont.dynamic)
                return;

            _textFont.RequestCharactersInTexture(_text, _fontSize, _textFontStyle);
        }

        #endregion

    }
}
