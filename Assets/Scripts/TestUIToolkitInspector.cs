using System.Collections.Generic;
using EasyToolkit.Inspector;
using EasyToolkit.Inspector.Attributes;
using UnityEngine;

[EasyInspector(InspectorBackendMode.UIToolkit)]
public class TestUIToolkitInspector : MonoBehaviour
{
    public int jj;
    public float bbb;
    public Color asd;
    public Transform Transform;

    public List<int> IntList;
}
