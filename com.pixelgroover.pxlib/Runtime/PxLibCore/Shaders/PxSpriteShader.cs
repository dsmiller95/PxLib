using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PxSpriteShader : MonoBehaviour
{
    //Base Properties
    MaterialPropertyBlock PropertyBlock { get; set; }
    public SpriteRenderer SpriteRenderer => _spriteRenderer ? _spriteRenderer : (_spriteRenderer = GetComponent<SpriteRenderer>());
    private SpriteRenderer _spriteRenderer;

    //Serialized Properties
    [SerializeField] [Range(0f, 1.0f)] float _opacity = 1;
    //Color Blending
    [SerializeField] bool _colorBlending;
    [SerializeField] Color _colorColor = Color.white;
    [SerializeField] [Range(0f, 1.0f)] float _colorIntensity = 0;
    //Additive Blending
    [SerializeField] bool _addBlending;
    [SerializeField] Color _addColor = Color.white;
    [SerializeField] [Range(0f, 1.0f)] float _addIntensity = 0;
    //Multiply Blending
    [SerializeField] bool _multiplyBlending;
    [SerializeField] Color _multiplyColor = Color.black;
    [SerializeField] [Range(0f, 1.0f)] float _multiplyIntensity = 0;
    //Grayscale Blending
    [SerializeField] bool _grayscaleBlending;
    [SerializeField] [Range(0f, 1.0f)] float _grayscaleIntensity = 0;
    //Transparency Cutoff
    [SerializeField] bool _transparencyCutoff;
    [SerializeField] [Range(0f, 1.0f)] float _transCutoffPos = 0;
    [SerializeField] [Range(0f, 1.0f)] float _transCutoffGradient = 0;

    //Sprite Sheet UV Data
    Vector4 _uvPositions;

    //Flash Attributes
    bool _flash;
    [Range(0f, 1.0f)] float _flashIntensity = 0;
    Color _flashColor = Color.white;
    float _flashEnterTime;
    float _flashStickTime;
    float _flashExitTime;

    //Frag Properties
    public float Opacity
    {
        get => _opacity;
        set
        {
            _opacity = value;
            UpdateSprite();
        }
    }
    public bool ColorBlending
    {
        get => _colorBlending;
        set
        {
            _colorBlending = value;
            UpdateSprite();
        }
    }
    public Color ColorColor
    {
        get => _colorColor;
        set
        {
            _colorColor = value;
            UpdateSprite();
        }
    }
    public float ColorIntensity
    {
        get => _colorIntensity;
        set
        {
            _colorIntensity = value;
            UpdateSprite();
        }
    }
    public bool AddBlending
    {
        get => _addBlending;
        set
        {
            _addBlending = value;
            UpdateSprite();
        }
    }
    public Color AddColor
    {
        get => _addColor;
        set
        {
            _addColor = value;
            UpdateSprite();
        }
    }
    public float AddIntensity
    {
        get => _addIntensity;
        set
        {
            _addIntensity = value;
            UpdateSprite();
        }
    }
    public bool MultiplyBlending
    {
        get => _multiplyBlending;
        set
        {
            _multiplyBlending = value;
            UpdateSprite();
        }
    }
    public Color MultiplyColor
    {
        get => _multiplyColor;
        set
        {
            _multiplyColor = value;
            UpdateSprite();
        }
    }
    public float MultiplyIntensity
    {
        get => _multiplyIntensity;
        set
        {
            _multiplyIntensity = value;
            UpdateSprite();
        }
    }
    public bool GrayscaleBlending
    {
        get => _grayscaleBlending;
        set
        {
            _grayscaleBlending = value;
            UpdateSprite();
        }
    }
    public float GrayscaleIntensity
    {
        get => _grayscaleIntensity;
        set
        {
            _grayscaleIntensity = value;
            UpdateSprite();
        }
    }
    public bool Flash
    {
        get => _flash;
        set
        {
            _flash = value;
            UpdateSprite();
        }
    }
    public float FlashIntensity
    {
        get => _flashIntensity;
        set
        {
            _flashIntensity = value;
            UpdateSprite();
        }
    }
    public Color FlashColor
    {
        get => _flashColor;
        set
        {
            _flashColor = value;
            UpdateSprite();
        }
    }
    public float FlashEnterTime
    {
        get => _flashEnterTime;
        set
        {
            _flashEnterTime = value;
            UpdateSprite();
        }
    }
    public float FlashStickTime
    {
        get => _flashStickTime;
        set
        {
            _flashStickTime = value;
            UpdateSprite();
        }
    }
    public float FlashExitTime
    {
        get => _flashExitTime;
        set
        {
            _flashExitTime = value;
            UpdateSprite();
        }
    }
    //Vertex Properties
    public bool TransparencyCutoff
    {
        get => _transparencyCutoff;
        set 
        {
            _transparencyCutoff = value;
            UpdateSprite(); 
        } 
        
    }
    public float TransCutoffPos
    {
        get => _transCutoffPos;
        set
        {
            _transCutoffPos = value;
            UpdateSprite();
        } 
    }
    public float TransCutoffAlpha
    {
        get => _transCutoffGradient;
        set
        {
            _transCutoffGradient = value;
            UpdateSprite();
        }
    }    

    //MonoBehaviour Methods

    private void Awake()
    {
        PropertyBlock = new MaterialPropertyBlock();
        UpdateSprite();
    }
    private void Update()
    {
        SetUvData();
        UpdateSprite();
    }
    private void OnValidate()
    {
        if (PropertyBlock == null)
        {
            PropertyBlock = new MaterialPropertyBlock();
        }
        UpdateSprite();
    }

    //Public Methods
    public void ResetShader()
    {
        Opacity = 1.0f;
        ColorBlending = false;
        AddBlending = false;
        MultiplyBlending = false;
        GrayscaleBlending = false;
        UpdateSprite();
    }
    public void FlashSprite(Color flashColor, float flashTime, float fadeTime, float stickTime, float intensity = 1f)
    {
        if (Flash)
        {
            StopAllCoroutines();
        }
        StartCoroutine(IEFlashSprite(flashColor, flashTime, fadeTime, stickTime, intensity));
    }
    public void FlashAndFadeSprite(Color flashColor, float flashTime, float fadeTime, float stickTime)
    {
        if (Flash)
        {
            StopAllCoroutines();
        }
        StartCoroutine(IEFlashAndFadeSprite(flashColor, flashTime, fadeTime, stickTime));
    }
    //private methods
    private IEnumerator IEFlashSprite(Color flashColor, float flashTime, float fadeTime, float stickTime, float intensity = 1f)
    {
        Flash = true;
        FlashColor = flashColor;
        for (float t = 0; t < flashTime; t += Time.deltaTime)
        {
            var tDerived = t / flashTime;
            FlashIntensity = tDerived * intensity;
            UpdateSprite();
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(stickTime);
        FlashIntensity = intensity;
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            var tDerived = t / fadeTime;
            FlashIntensity = (1 - tDerived) * intensity;
            UpdateSprite();
            yield return new WaitForEndOfFrame();
        }
        FlashIntensity = 0;        
        Flash = false;
        UpdateSprite();
        //Debug.Log("Done Flashing Sprite");
    }
    private IEnumerator IEFlashAndFadeSprite(Color flashColor, float flashTime, float fadeTime, float stickTime)
    {
        Flash = true;
        Opacity = 1;
        FlashColor = flashColor;
        for (float t = 0; t < flashTime; t += Time.deltaTime)
        {
            var tDerived = t / flashTime;
            FlashIntensity = tDerived;
            UpdateSprite();
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(stickTime);
        FlashIntensity = 1;
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            var tDerived = t / fadeTime;
            Opacity = 1 - tDerived;
            UpdateSprite();
            yield return new WaitForEndOfFrame();
        }
        FlashIntensity = 0;
        Opacity = 0;
        Flash = false;
        UpdateSprite();
    }    
    private void SetUvData()
    {
        if (SpriteRenderer.sprite)
        {
            var dimensions = new Vector2(SpriteRenderer.sprite.texture.width, SpriteRenderer.sprite.texture.height);
            var rect = SpriteRenderer.sprite.rect;
            //get minMax x and y
            float xMin = rect.x / dimensions.x;
            float xMax = xMin + rect.width / dimensions.x;
            float yMin = rect.y / dimensions.y;
            float yMax = yMin + rect.height / dimensions.y;
            _uvPositions = new Vector4(xMin, xMax, yMin, yMax);
            //Debug.Log(_uvPositions);
        }
    }
    private void UpdateSprite()
    {
        SetUvData();
        SpriteRenderer.GetPropertyBlock(PropertyBlock);
            PropertyBlock.SetVector("_UvPositions", _uvPositions);
            PropertyBlock.SetFloat("_Opacity", _opacity);
            PropertyBlock.SetFloat("_AddBlending", _addBlending ? 1 : 0);
            PropertyBlock.SetColor("_AddColor", _addColor);
            PropertyBlock.SetFloat("_AddIntensity", _addIntensity);
            PropertyBlock.SetFloat("_ColorBlending", _colorBlending ? 1 : 0);
            PropertyBlock.SetColor("_ColorColor", _colorColor);
            PropertyBlock.SetFloat("_ColorIntensity", _colorIntensity);
            PropertyBlock.SetFloat("_MultiplyBlending", _multiplyBlending ? 1 : 0);
            PropertyBlock.SetColor("_MultiplyColor", _multiplyColor);
            PropertyBlock.SetFloat("_MultiplyIntensity", _multiplyIntensity);
            PropertyBlock.SetFloat("_GrayscaleBlending", _grayscaleBlending ? 1 : 0);
            PropertyBlock.SetFloat("_GrayscaleIntensity", _grayscaleIntensity);
            PropertyBlock.SetFloat("_Flash", _flash ? 1 : 0);
            PropertyBlock.SetColor("_FlashColor", _flashColor);
            PropertyBlock.SetFloat("_FlashIntensity", _flashIntensity);
            PropertyBlock.SetFloat("_TransparencyCutoff", _transparencyCutoff ? 1 : 0);
            PropertyBlock.SetFloat("_TransCutoffPos", _transCutoffPos);
            PropertyBlock.SetFloat("_TransCutoffGradient", _transCutoffGradient);
        SpriteRenderer.SetPropertyBlock(PropertyBlock);
    }
}
