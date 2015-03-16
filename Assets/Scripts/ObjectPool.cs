using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
public class ObjectPool : MonoBehaviour
{  
    public static ObjectPool Instance;	

	public Transform GameSceneParent;
    public GameObject[] ObjectPrefabs;
    
	public List<GameObject>[] PooledObjects;
    
	private List<GameObject> _UnPooledObjects;
	Dictionary<int, string>  _CachedLookup;
       
    public int[] AmountToBuffer;   
    public int   DefaultBufferAmount = 3;
       
    public GameObject ContainerObject;

    void Awake ()
    {
        Instance = this;
    }
   
    // Use this for initialization
    void Start ()
    {    	
    }

	/// <summary>
	/// Init this instance.
	/// </summary>
	public void Init ()
	{		       
		PooledObjects = new List<GameObject>[ObjectPrefabs.Length];

        _CachedLookup = new Dictionary<int, string>();
		_UnPooledObjects = new List<GameObject>();
		
		if(ContainerObject == null)
		{
			ContainerObject = new GameObject("ObjectPool");
			ContainerObject.transform.parent = GameSceneParent;
		
	        int i = 0;

	        foreach (GameObject objectPrefab in ObjectPrefabs )
	        {
	            PooledObjects[i] = new List<GameObject>(); 
	           
				_CachedLookup[objectPrefab.GetInstanceID()] = objectPrefab.name;
	            
				int bufferAmount;
	           
	            if (i < AmountToBuffer.Length)
				{
					bufferAmount = AmountToBuffer[i];
				}
	            else
				{
	                bufferAmount = DefaultBufferAmount;
				}
				
	            for (int n=0; n < bufferAmount; n++)
	            {
	                GameObject newObj = Instantiate(objectPrefab) as GameObject;
	                newObj.name = objectPrefab.name;
					_CachedLookup[newObj.GetInstanceID()] = newObj.name;
	                PoolObject(newObj);
	            }
	           
	            i++;
	        }
		}
		else
		{
	        int i = 0;
	        foreach (GameObject objectPrefab in ObjectPrefabs )
	        {
	            PooledObjects[i] = new List<GameObject>(); 	           
				_CachedLookup[objectPrefab.GetInstanceID()] = objectPrefab.name;
	           
	            i++;
	        }		
			
	        for(int j = 0; j < ContainerObject.transform.childCount; j++)
			{
				GameObject newObj = ContainerObject.transform.GetChild(j).gameObject;
				_CachedLookup[newObj.GetInstanceID()] = newObj.name;
				PoolObject(newObj);
			}
		}
	}

    /// <summary>
    /// Gets a new object for the name type provided.  If no object type exists or if onlypooled is true and there is no objects of that type in the pool
    /// then null will be returned.
    /// </summary>
    /// <returns>
    /// The object for type.
    /// </returns>
    /// <param name='objectType'>
    /// Object type.
    /// </param>
    /// <param name='onlyPooled'>
    /// If true, it will only return an object if there is one currently pooled.
    /// </param>
    public GameObject GetObjectForType ( string objectType , bool onlyPooled = true)
    {
        for(int i=0; i < ObjectPrefabs.Length; i++)
        {
            GameObject prefab = ObjectPrefabs[i];
            
			if (_CachedLookup[prefab.GetInstanceID()] == objectType)
            {
                if (PooledObjects[i].Count > 0)
                {
                    GameObject pooledObject = PooledObjects[i][0];
                    PooledObjects[i].RemoveAt(0);                    
					pooledObject.transform.parent = GameSceneParent;
					pooledObject.SetActive(true);
					_UnPooledObjects.Add(pooledObject);
					
					return pooledObject;
                   
                }
				else if (!onlyPooled)
				{
					GameObject obj = Instantiate(ObjectPrefabs[i]) as GameObject;
					_UnPooledObjects.Add(obj);
					_CachedLookup[obj.GetInstanceID()] = objectType;
                    return obj;
                }
               
               // break;
               
            }
        }
           
        //If we have gotten here either there was no object of the specified type or non were left in the pool with onlyPooled set to true
        return null;
    }

    /// <summary>
    /// Pools the object specified.  Will not be pooled if there is no prefab of that type.
    /// </summary>
    /// <param name='obj'>
    /// Object to be pooled.
    /// </param>
    public bool PoolObject ( GameObject obj )
    {
        for (int i=0; i < ObjectPrefabs.Length; i++)
        {            
			if (_CachedLookup[ObjectPrefabs[i].GetInstanceID()] == _CachedLookup[obj.GetInstanceID()])
            {
                obj.SetActive(false);
                obj.transform.parent = ContainerObject.transform;
                PooledObjects[i].Add(obj);
                return true;
            }
        }

		return false;
    }
	
	/// <summary>
	/// Pools all objects.
	/// </summary>
	/// <param name='onlyPooled'>
	/// If true, it will only pool objects from the initial buffer.
	/// </param>
	public void PoolAllObjects ( bool onlyPooled = true )
	{
		foreach (GameObject obj in _UnPooledObjects)
		{
			if (!onlyPooled && obj.name.Contains("(Clone)"))
			{
				obj.name = obj.name.Substring(0, obj.name.Length - 7);
			}

			if (!PoolObject(obj))
			{
				obj.SetActive(false);
				Object.Destroy(obj);
			}
		}
	}
}