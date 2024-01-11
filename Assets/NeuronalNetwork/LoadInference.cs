using System.Collections;
using System.Collections.Generic;
using Unity.Barracuda;
using UnityEngine;

public class LoadInference : MonoBehaviour {
    //----------------------------------------------------------

    public NNModel modelAsset;
    private IWorker worker;
    private Model runtimeModel;

    public int month;
    public float methane, co2;
            
    float[] meanScaler = new float[] { 6.5f, 1376.95894169f, 321.43283854f };
    float[] deviationScaler = new float[]{ 3.45205253f, 326.40183747f,  40.22928149f };

    //----------------------------------------------------------
    
    void Start() {
        runtimeModel = ModelLoader.Load(modelAsset);
        worker = WorkerFactory.CreateWorker(runtimeModel);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            Predict(this.month, this.methane, this.co2);
        }
    }
    
    public void nextTurn()
    {
        GameManager.instance.nextTurn();
        Predict(GameManager.instance.month, GameManager.instance.ch4, GameManager.instance.co2);
    }

    void OnDestroy() {
        worker?.Dispose();
    }
    
    //----------------------------------------------------------

    void Predict(int monthValue, float methaneValue, float co2Value) {
        Tensor inputTensor = new Tensor(1, 3, ScalerKerasValues(new float[] { monthValue, methaneValue, co2Value }));

        worker.Execute(inputTensor);
        Tensor output = worker.PeekOutput();

        float globalTemp = output[0, 0];
        float seaLevel = output[0, 1];

        Debug.Log($"Predicted Global Temp: {globalTemp}, Predicted Sea Level: {seaLevel}");

        GameManager.instance.temp = globalTemp;
        GameManager.instance.sealvl = seaLevel;

        inputTensor.Dispose();
        output.Dispose();
    }

    //----------------------------------------------------------

    public float[] ScalerKerasValues(float[] originalValues) {
        float[] scaledValues = new float[originalValues.Length];
        for (int i = 0; i < originalValues.Length; i++) {
            scaledValues[i] = (originalValues[i] - meanScaler[i]) / deviationScaler[i];
        }
        return scaledValues;
    }

}
