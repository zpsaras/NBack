using UnityEngine;
using System.Collections;

public class JSONWriter : MonoBehaviour {

	public static JSONObject BuildJSON(Letter[] zeroBack, Letter[] oneBack, Letter[] twoBack, Letter[] threeBack)
    {
        JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
        int i;

        json.AddField("TaskType", "NBack");

        JSONObject zeba = new JSONObject(JSONObject.Type.ARRAY);
        for (i = 0; i < zeroBack.Length; i++)
        {
            JSONObject arr = new JSONObject(JSONObject.Type.ARRAY);
            arr.AddField("Character", zeroBack[i].getChar().ToString());
            arr.AddField("Target", zeroBack[i].isTarget().ToString());
            arr.AddField("Lure", zeroBack[i].isLure().ToString());
            arr.AddField("Response", zeroBack[i].getResponse().ToString());
            arr.AddField("ResponseTime", zeroBack[i].GetResponseTime().ToString());
            zeba.AddField(i.ToString(), arr);
        }
        json.AddField("ZeroBack", zeba);

        JSONObject onba = new JSONObject(JSONObject.Type.ARRAY);
        for (i = 0; i < zeroBack.Length; i++)
        {
            JSONObject arr = new JSONObject(JSONObject.Type.ARRAY);
            arr.AddField("Character", oneBack[i].getChar().ToString());
            arr.AddField("Target", oneBack[i].isTarget().ToString());
            arr.AddField("Lure", oneBack[i].isLure().ToString());
            arr.AddField("Response", oneBack[i].getResponse().ToString());
            arr.AddField("ResponseTime", oneBack[i].GetResponseTime().ToString());
            zeba.AddField(i.ToString(), arr);
        }
        json.AddField("OneBack", onba);

        JSONObject toba = new JSONObject(JSONObject.Type.ARRAY);
        for (i = 0; i < zeroBack.Length; i++)
        {
            JSONObject arr = new JSONObject(JSONObject.Type.ARRAY);
            arr.AddField("Character", twoBack[i].getChar().ToString());
            arr.AddField("Target", twoBack[i].isTarget().ToString());
            arr.AddField("Lure", twoBack[i].isLure().ToString());
            arr.AddField("Response", twoBack[i].getResponse().ToString());
            arr.AddField("ResponseTime", twoBack[i].GetResponseTime().ToString());
            zeba.AddField(i.ToString(), arr);
        }
        json.AddField("TwoBack", toba);

        JSONObject teba = new JSONObject(JSONObject.Type.ARRAY);
        for (i = 0; i < zeroBack.Length; i++)
        {
            JSONObject arr = new JSONObject(JSONObject.Type.ARRAY);
            arr.AddField("Character", threeBack[i].getChar().ToString());
            arr.AddField("Target", threeBack[i].isTarget().ToString());
            arr.AddField("Lure", threeBack[i].isLure().ToString());
            arr.AddField("Response", threeBack[i].getResponse().ToString());
            arr.AddField("ResponseTime", threeBack[i].GetResponseTime().ToString());
            zeba.AddField(i.ToString(), arr);
        }
        json.AddField("ThreeBack", teba);

        return json;
    }
}
