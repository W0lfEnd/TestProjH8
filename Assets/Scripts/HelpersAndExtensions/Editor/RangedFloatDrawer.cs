using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer(typeof(RangedFloat), true)]
public class RangedFloatDrawer : PropertyDrawer
{
  public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
  {
    label    = EditorGUI.BeginProperty( position, label, property );
    position = EditorGUI.PrefixLabel( position, label );

    SerializedProperty min_prop = property.FindPropertyRelative( "min" );
    SerializedProperty max_prop = property.FindPropertyRelative( "max" );

    float minValue = min_prop.floatValue;
    float maxValue = max_prop.floatValue;

    float rangeMin = 0.0f;
    float rangeMax = 10.0f;

    MinMaxRangeAttribute[] ranges = (MinMaxRangeAttribute[])fieldInfo.GetCustomAttributes( typeof(MinMaxRangeAttribute), true );
    if ( ranges.Length > 0 )
    {
      rangeMin = ranges[0].Min;
      rangeMax = ranges[0].Max;
    }

    const float rangeBoundsLabelWidth = 40f;

    Rect rangeBoundsLabel1Rect = new Rect( position );
    rangeBoundsLabel1Rect.width = rangeBoundsLabelWidth;
    GUI.Label( rangeBoundsLabel1Rect, new GUIContent( minValue.ToString( "F2" ) ) );
    position.xMin += rangeBoundsLabelWidth;

    Rect rangeBoundsLabel2Rect = new Rect( position );
    rangeBoundsLabel2Rect.xMin = rangeBoundsLabel2Rect.xMax - rangeBoundsLabelWidth;
    GUI.Label( rangeBoundsLabel2Rect, new GUIContent( maxValue.ToString( "F2" ) ) );
    position.xMax -= rangeBoundsLabelWidth;

    EditorGUI.BeginChangeCheck();
    EditorGUI.MinMaxSlider( position, ref minValue, ref maxValue, rangeMin, rangeMax );
    if ( EditorGUI.EndChangeCheck() )
    {
      min_prop.floatValue = minValue;
      max_prop.floatValue = maxValue;
    }

    EditorGUI.EndProperty();
  }
}
