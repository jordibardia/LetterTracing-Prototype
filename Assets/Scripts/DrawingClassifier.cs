using Microsoft.ML;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrawingClassifier : MonoBehaviour
{
    public void Classify(float[] sample)
    {
        var context = new MLContext();
        var session = new InferenceSession("Assets/MLModels/model.onnx");
        int[] dims = new int[] {1, 28, 28, 1 };
        var tensor = new DenseTensor<float>(sample, dims);
        var xs = new List<NamedOnnxValue>()
        {
            NamedOnnxValue.CreateFromTensor<float>("keras_tensor", tensor),
        };

        var results = session.Run(xs).ToArray();
        //Debug.Log(output);
        Debug.Log("Finished detection");
    }
}
