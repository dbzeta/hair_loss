using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class UtilsClass
{
    static public string GetString(params string[] param)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < param.Length; i++)
            sb.Append(param[i]);
        return sb.ToString();
    }
    static public string GetString(string[] param, string split = null)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < param.Length; i++)
        {
            string add = split;
            if (i == param.Length - 1)
                add = string.Empty;

            sb.Append(param[i] + add);
        }
        return sb.ToString();
    }


    static public List<Dictionary<string, string>> ConvertCSVFileToJsonObject(string CSVText)
    {
        // TextAsset textAsset = Resources.Load<TextAsset>(filePath + fileName) as TextAsset;
        string text = CSVText.Replace("\r", "");
        var lines = text.Split('\n');
        var properties = lines[0].Split(',');

        var csv = new List<string[]>();
        foreach (string line in lines)
        {
            if (string.IsNullOrEmpty(line))
                continue;

            string[] checkSplits = line.Split(',');
            string[] data = line.Split(',');
            if (properties.Length != checkSplits.Length)
            {
                data = new string[properties.Length];
                List<string> contextList = new List<string>();
                bool bString = false;
                int idx = 0;
                for (int i = 0; i < checkSplits.Length; i++)
                {
                    string split = checkSplits[i];
                    int start_idx = 0;
                    if (split.Length > start_idx && split[start_idx] == '\"')
                    {
                        bString = true;
                    }

                    if (bString)
                    {
                        contextList.Add(split);
                        int last_idx = checkSplits[i].Length - 1;
                        if (split[last_idx] == '\"')
                        {
                            bString = false;
                            string seperator = ",";
                            string join = string.Join(seperator, contextList);
                            data[idx++] = join;
                            contextList.Clear();
                        }
                    }
                    else
                       data[idx++] = split;
                }
            }
            csv.Add(data);
            Debug.Log(UtilsClass.GetString(data));
        }

        var listObjResult = new List<Dictionary<string, string>>();
        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrEmpty(lines[i]))
                continue;

            var objResult = new Dictionary<string, string>();
            for (int j = 0; j < properties.Length; j++)
            {
                if (properties.Length == csv[i].Length)
                {
                    objResult.Add(properties[j], csv[i][j]);
                    // Debug.Log(csv[i][j]);
                }
            }

            if (objResult.Count > 0)
            {
                listObjResult.Add(objResult);
            }
        }
        return listObjResult;
    }

    public static string GetUniqueID()
    {
        var random = new System.Random();
        DateTime epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
        double timestamp = (System.DateTime.UtcNow - epochStart).TotalSeconds;

        string lang = ((int)Application.systemLanguage).ToString();
        string device = ((int)Application.platform).ToString();

        string uniqueID = lang                                                              //Language
                + "-" + device                                                              //Device    
                + "-" + String.Format("{0:X}", Convert.ToInt32(timestamp))                  //Time
                + "-" + String.Format("{0:X}", Convert.ToInt32(Time.time * 10))        //Time in game
                + "-" + String.Format("{0:X}", random.Next(10000));                    //random number

        uniqueID = uniqueID.Substring(0, 20);
        Debug.Log("Generated Unique ID: " + uniqueID);


        return uniqueID;
    }

    static public Color GetColorByHexCode(string _hexCode)
    {
        Color color = Color.white;
        string[] HexCodes = _hexCode.Split('#');
        if (HexCodes.Length <= 1) _hexCode = GetString("#", _hexCode);
        if (_hexCode.Length <= 7) _hexCode = GetString(_hexCode, "FF");
        ColorUtility.TryParseHtmlString(_hexCode, out color);
        return color;
    }
    static public string GetHexCodeByColor(Color _color)
    {
        return ColorUtility.ToHtmlStringRGBA(_color);
    }

    static public void ShuffleList<T>(ref List<T> list)
    {
        int random1;
        int random2;
        T tmp;
        for (int index = 0; index < list.Count; ++index)
        {
            random1 = UnityEngine.Random.Range(0, list.Count);
            random2 = UnityEngine.Random.Range(0, list.Count);

            tmp = list[random1];
            list[random1] = list[random2];
            list[random2] = tmp;
        }
    }
    static public void ShuffleList<T>(ref T[] arr)
    {
        int random1;
        int random2;
        T tmp;
        for (int index = 0; index < arr.Length; ++index)
        {
            random1 = UnityEngine.Random.Range(0, arr.Length);
            random2 = UnityEngine.Random.Range(0, arr.Length);

            tmp = arr[random1];
            arr[random1] = arr[random2];
            arr[random2] = tmp;
        }
    }
    static public int GetRandomRange(int minInclusive, int maxExclusive)
    {
        return UnityEngine.Random.Range(minInclusive, maxExclusive);
    }
    static public float GetRandomRange(float minInclusive, float maxExclusive)
    {
        return UnityEngine.Random.Range(minInclusive, maxExclusive);
    }

    public static List<int> GetRandIdxList(int _randAmount, int randomLength)
    {
        List<int> IdxList = new List<int>();

        for (int i = 0; i < _randAmount; i++)
        {
            int loopCnt = 0;
            while (true)
            {
                int rand = UnityEngine.Random.Range(0, randomLength);
                bool Same = false;

                for (int j = 0; j < IdxList.Count; j++)
                {
                    if (IdxList[j] == rand)
                    {
                        Same = true;
                        break;
                    }
                }

                if (Same == false)
                {
                    IdxList.Add(rand);
                    break;
                }

                loopCnt++;

                if (loopCnt > 1000)
                    break;
            }
        }

        return IdxList;
    }
    static public int GetRandomIndex(int _count)
    {
        return UnityEngine.Random.Range(0, _count);
    }
    static public int[] GetRandomIndex(int _count, int _max, bool _bCheckDuplication)
    {
        int[] randArray = new int[_count];
        for (int i = 0; i < randArray.Length; i++)
        {
            randArray[i] = -1;
        }

        for (int i = 0; i < _count;)
        {
            int random = GetRandomIndex(_max);
            if (_bCheckDuplication)
            {
                bool isSame = false;
                for (int k = 0; k < randArray.Length; k++)
                {
                    if (randArray[k] == random)
                    {
                        isSame = true;
                        break;
                    }
                }

                if (isSame == false)
                {
                    randArray[i] = random;
                    i++;
                }
            }
            else
            {
                randArray[i] = random;
                i++;
            }

            if (i >= _count)
                break;
        }

        return randArray;
    }
    static public int GetRandomIndex(List<float> inputDatas, float _offset = 0)
    {
        // Random random = new Random(seed);
        float total = 0;
        for (int i = 0; i < inputDatas.Count; i++)
        {
            total += inputDatas[i];
        }

        float random = UnityEngine.Random.Range(0.0f, 1.0f);
        float pivot = (random + (_offset / 100.0f)) * total;


        for (int i = 0; i < inputDatas.Count; i++)
        {
            if (pivot < inputDatas[i])
            {
                return i;
            }
            else
            {
                pivot -= inputDatas[i];
            }
        }
        return (inputDatas.Count - 1);
    }

    static public bool GetRandomResult(float _fProbability)
    {
        return (UnityEngine.Random.Range(0.0f, 1.0f) * 100) < _fProbability;
    }
    static public Vector2 GetInsideUnitCircle(float radius)
    {
        return UnityEngine.Random.insideUnitCircle * radius;
    }

    static public T ParseToType<T>(string _value)
    {
        int tryInt = 0;

        if (typeof(T) == typeof(string))
        {
            string covert = _value.Replace("\"", string.Empty);
            covert = covert.Replace("\\r\\n", "\n");
            covert = covert.Replace("\\n", "\n");
            return (T)Convert.ChangeType(covert, typeof(T));
        }
        if (typeof(T) == typeof(int))
        {
            int.TryParse(_value, out tryInt);
            return (T)Convert.ChangeType(tryInt, typeof(T));
        }
        float tryfloat = 0.0f;
        if (typeof(T) == typeof(float))
        {
            float.TryParse(_value, out tryfloat);
            return (T)Convert.ChangeType(tryfloat, typeof(T));
        }
        double trydouble = 0.0f;
        if (typeof(T) == typeof(double))
        {
            double.TryParse(_value, out trydouble);
            return (T)Convert.ChangeType(trydouble, typeof(T));
        }
        if (typeof(T).IsEnum)
        {
            T tryEnum = (T)Enum.Parse(typeof(T), _value);
            return (T)Convert.ChangeType(tryEnum, typeof(T));
        }
        return default(T);
    }

    static public T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }

    static public List<T> CheckSurroundings<T>(List<T> list)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            var item = list[i];
            UnityEngine.Object obj = item as UnityEngine.Object;
            if (obj.IsRealNull())
            {
                list.RemoveAt(i);
            }
        }

        return list;
    }
    public static Vector3[] GetQuadraticBezierPoints(Vector3 startpoint, Vector3 endPoint, float maxTime, Vector3 curveDir, float curveHeigh)
    {
        Vector3 heighPoint = startpoint + (endPoint - startpoint) / 2 + (curveDir * curveHeigh);

        Vector3[] res = new Vector3[(int)(maxTime / 0.01f)];
        int index = 0;

        if (maxTime <= 0) maxTime = 1f;
        float speed = 1f / maxTime;
        for (float t = 0; t <= 1f; t += (0.01f * speed))
        {
            Vector3 newPoint = (Mathf.Pow(1 - t, 2) * startpoint) + (2 * (1 - t) * t * heighPoint) + (t * t * endPoint);
            try
            {
                res[index++] = newPoint;
            }
            catch
            {
                break;
            }
        }
        return res;
    }

    static public bool IsRealNull(this UnityEngine.Object obj)
    {
        bool bIsNull = false;
        if (IsNull(obj))
        {
            bIsNull = true;
            DebugLog("obj is null.");
        }
        else
        {
            // Public or SerializeField is Fake Null
            if (IsFakeNull(obj))
            {
                bIsNull = false;
                DebugLog("obj is fake null.");
            }
            else
            {
                bIsNull = true;
                DebugLog("obj is not fake null.");
            }
        }
        return bIsNull;
    }
    public static bool IsNull(this UnityEngine.Object obj)
    {
        return ReferenceEquals(obj, null);
    }
    public static bool IsFakeNull(this UnityEngine.Object obj)
    {
        return !ReferenceEquals(obj, null) && obj;
    }
    public static bool IsAssigned(this UnityEngine.Object obj)
    {
        return obj;
    }
    public static void DebugLog(object message)
    {
        if (Debug.isDebugBuild)
            Debug.Log(message);
    }
    public static void DebugLog(object message, UnityEngine.Object context)
    {
        if (Debug.isDebugBuild)
            Debug.Log(message, context);
    }

    public static string ConvertDoubleToInGameUnit(double _dValue)
    {
        string zero = "0";

        if (-1d < _dValue && _dValue < 1d)
        {
            return _dValue.ToString();
        }

        if (double.IsInfinity(_dValue))
        {
            return "Infinity";
        }

        //  ��ȣ ��� ���ڿ�
        string significant = (_dValue < 0) ? "-" : string.Empty;

        //  ������ ����
        string showNumber = string.Empty;

        //  ���� ���ڿ�
        string unitString = string.Empty;

        //  ������ �ܼ�ȭ ��Ű�� ���� ������ ���� ǥ�������� ������ �� ó��
        string[] partsSplit = (_dValue.ToString("E")).Split('+');

        //  ����
        if (partsSplit.Length < 2)
        {
            return zero;
        }

        //  ���� (�ڸ��� ǥ��)
        if (!int.TryParse(partsSplit[1], out int exponent))
        {
            Debug.LogWarningFormat("Failed - ToCurrentString({0}) : partSplit[1] = {1}", _dValue, partsSplit[1]);
            return zero;
        }

        //  ���� ���ڿ� �ε���
        int quotient = exponent / 4;

        //  �������� ������ �ڸ��� ��꿡 ���(10�� �ŵ������� ���)
        int remainder = exponent % 4;

        //  1�� �̸��� �׳� ǥ��
        if (exponent < 4)
        {
            string strValue = _dValue.ToString();
            string[] dotSplit = (strValue).Split('.');
            if (dotSplit.Length > 1)
            {
                // 100.01
                if ((dotSplit[0].Length + dotSplit[1].Length) <= 5) showNumber = strValue;
                else
                {
                    showNumber = string.Format("{0}.{1}1", dotSplit[0], dotSplit[1].Substring(0, 5 - dotSplit[0].Length - 1));
                }
            }
            else
                showNumber = System.Math.Truncate(_dValue).ToString();
        }
        else
        {
            //  10�� �ŵ������� ���ؼ� �ڸ��� ǥ������ ����� �ش�.
            var temp = double.Parse(partsSplit[0].Replace("E", "")) * System.Math.Pow(10, remainder);

            //  �Ҽ� ��°�ڸ������� ����Ѵ�.
            showNumber = temp.ToString("F").Replace(".00", "");
        }

        unitString = GetInGameUnit(quotient);

        string[] showNumbers = showNumber.Split('.');
        if (showNumbers != null && showNumbers.Length == 2)
        {
            if (string.IsNullOrEmpty(unitString))
                return string.Format("{0}{1}.{2}", significant, showNumbers[0], showNumbers[1]);
            else
                return string.Format("{0}{1}{2}{3}", significant, showNumbers[0], unitString, showNumbers[1]);
        }
        else
            return string.Format("{0}{1}{2}", significant, showNumber, unitString);

    }



    static string GetInGameUnit(int _unitCount)
    {
        switch (_unitCount)
        {
            case 0: return string.Empty;
            case 1: return "��";
            case 2: return "��";
            case 3: return "��";
            case 4: return "��";
            case 5: return "��";
            case 6: return "��";
            case 7: return "��";
            case 8: return "��";
            case 9: return "��";
            case 10: return "��";
            case 11: return "��";
            case 12: return "��";
            case 13: return "���ϻ�";
            case 14: return "�ƽ±�";
            case 15: return "����Ÿ";
            case 16: return "�Ұ�����";
            case 17: return "�������";
            case 18: return "��";
            case 19: return "��";
            default:        
                break;      
        }

        return string.Empty;

        if (_unitCount >= 0 && _unitCount < 1) return string.Empty;
        else if (_unitCount >= 5 && _unitCount < 9) return "��";
        else if (_unitCount >= 9 && _unitCount < 13) return "��";
        else if (_unitCount >= 13 && _unitCount < 17) return "��";
        else if (_unitCount >= 17 && _unitCount < 21) return "��";
        else if (_unitCount >= 21 && _unitCount < 25) return "��";
        else if (_unitCount >= 25 && _unitCount < 29) return "��";
        else if (_unitCount >= 29 && _unitCount < 33) return "��";
        else if (_unitCount >= 33 && _unitCount < 37) return "��";
        else if (_unitCount >= 37 && _unitCount < 41) return "��";
        else if (_unitCount >= 41 && _unitCount < 45) return "��";
        else if (_unitCount >= 45 && _unitCount < 49) return "��";
        else if (_unitCount >= 49 && _unitCount < 53) return "��";
        else if (_unitCount >= 53 && _unitCount < 57) return "���ϻ�";
        else if (_unitCount >= 57 && _unitCount < 61) return "�ƽ±�";
        else if (_unitCount >= 61 && _unitCount < 65) return "����Ÿ";
        else if (_unitCount >= 65 && _unitCount < 69) return "�Ұ�����";
        else if (_unitCount >= 69 && _unitCount < 71) return "�������";
        else if (_unitCount >= 71 && _unitCount < 75) return "��";
        else if (_unitCount >= 75 ) return "��";

        return string.Empty;
    }
}
