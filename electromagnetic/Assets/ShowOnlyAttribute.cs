using UnityEngine;
 
/// <summary>
/// Display a field as Show-only in the inspector.
/// CustomPropertyDrawers will not work when this attribute is used.
/// </summary>
/// <seealso cref="BeginShowOnlyGroupAttribute"/>
/// <seealso cref="EndShowOnlyGroupAttribute"/>
public class ShowOnlyAttribute : PropertyAttribute { }
 
/// <summary>
/// Display one or more fields as Show-only in the inspector.
/// Use <see cref="EndShowOnlyGroupAttribute"/> to close the group.
/// Works with CustomPropertyDrawers.
/// </summary>
/// <seealso cref="EndShowOnlyGroupAttribute"/>
/// <seealso cref="ShowOnlyAttribute"/>
public class BeginShowOnlyGroupAttribute : PropertyAttribute { }
 
/// <summary>
/// Use with <see cref="BeginShowOnlyGroupAttribute"/>.
/// Close the Show-only group and resume editable fields.
/// </summary>
/// <seealso cref="BeginShowOnlyGroupAttribute"/>
/// <seealso cref="ShowOnlyAttribute"/>
public class EndShowOnlyGroupAttribute : PropertyAttribute { }