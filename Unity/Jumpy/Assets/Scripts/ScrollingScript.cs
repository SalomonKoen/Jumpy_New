using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ScrollingScript : MonoBehaviour
{
	public static Vector2 speed = new Vector2(0, 0);

	public static float height = 0;

    public float speedMultiplier = 1f;
	public Vector2 direction = new Vector2(0, -1);

	public bool isLooping = false;
	public bool isLinkedToCamera = false;

    private bool firstUp = false;
    private Vector2 firstPos = Vector2.zero;

    public static bool paused = false;
	public bool fillScreen = false; 

	public float margin = 0f;
	public bool alignLeft = false;
	public bool alignRight = false;

	private List<Transform> backgroundPart;

	private float screenWidth;
	private float screenHeight;

	public static bool isLeftTrans = false;
	public static bool isRightTrans = false;
	public static bool isBackTrans = false;

	public static bool isTransition = false;

	private Transform transition;
	private Transform background;

	void Start()
	{
		isLeftTrans = false;
		isRightTrans = false;
		isBackTrans = false;
		isTransition = false;
		paused = false;
		speed = new Vector2(0, 0);
		if (isLooping)
		{
			backgroundPart = new List<Transform>();

			float pos = 0;
			screenWidth = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth, 0)).x*2;
			screenHeight = Camera.main.ScreenToWorldPoint(new Vector2(0, Camera.main.pixelHeight)).y;
			Vector3 dirNorm = direction.normalized;

			for (int i = 0 ; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);

				float width = child.renderer.bounds.size.x;
				float height = child.renderer.bounds.size.y;

				if (child.renderer != null)
				{
					backgroundPart.Add(child);
				}

                if (dirNorm.y == -1)
                {
					if (fillScreen)
					{
						float scale = screenWidth/width;
						child.localScale = new Vector3(scale, scale);
						child.position = new Vector3(0, child.renderer.bounds.size.y/2 + pos, child.position.z);
						pos += child.renderer.bounds.size.y;
					}
					else
					{
						if (alignLeft)
						{
							child.position = new Vector3(-screenWidth/2 + width/2 - margin, height/2 + pos, child.position.z);
						}
						else if (alignRight)
						{
							child.position = new Vector3(screenWidth/2 - width/2 + margin, height/2 + pos, child.position.z);
						}
						else
							child.position = new Vector3(child.position.x, height/2 + pos, child.position.z);

						pos += height;
					}
                }
			}

			if (dirNorm.y == -1)
			{
				while (pos < 2*screenHeight && backgroundPart.Count != 0)
				{
					Transform last = backgroundPart.Last<Transform>();
					Transform t = (Transform)Instantiate(last, new Vector3(0, 0), Quaternion.identity);
					float height = t.renderer.bounds.size.y;
					t.position = new Vector3(last.position.x, t.renderer.bounds.size.y/2 + pos, last.position.z);
					t.parent = transform;
					pos += height;

					backgroundPart.Add(t);
				}

				backgroundPart = backgroundPart.OrderByDescending(t => t.position.x).ToList();
			}
		}
	}

	public void Transition(GameObject transition, GameObject background)
	{
		isTransition = true;

		if (fillScreen)
		{
			isBackTrans = true;
		}
		else if (alignLeft)
		{
			isLeftTrans = true;
		}
		else if (alignRight)
		{
			isRightTrans = true;
		}

		transition.transform.parent = transform;
		background.transform.parent = transform;

		Vector3 dirNorm = direction.normalized;
		float pos = backgroundPart.Last().position.y + backgroundPart.Last().renderer.bounds.size.y/2;

		if (dirNorm.y == -1)
		{
			for (int i = 0; i < 2; i++)
			{
				Transform child = null;

				if (i == 0)
				{
					child = transition.transform;
					this.transition = child;
				}
				else
				{
					child = background.transform;
					this.background = child;
				}

				backgroundPart.Add(child);
				
				float width = child.renderer.bounds.size.x;
				float height = child.renderer.bounds.size.y;

				if (fillScreen)
				{
					float scale = screenWidth/width;
					child.localScale = new Vector3(scale, scale);
					child.position = new Vector3(0, child.renderer.bounds.size.y/2 + pos, child.position.z);
					pos += child.renderer.bounds.size.y;
				}
				else
				{
					if (alignLeft)
					{
						child.position = new Vector3(-screenWidth/2 + width/2 - margin, height/2 + pos, child.position.z);
					}
					else if (alignRight)
					{
						child.position = new Vector3(screenWidth/2 - width/2 + margin, height/2 + pos, child.position.z);
					}
					else
						child.position = new Vector3(child.position.x, height/2 + pos, child.position.z);
					
					pos += height;
				}
			}
		}
	}

	public void Restart()
	{
		Start();
	}
    
    void FixedUpdate()
	{
        if (!paused)
        {
            if (!isLinkedToCamera)
            {
				Vector3 movement = new Vector3(speed.x * direction.x * speedMultiplier, speed.y * direction.y * speedMultiplier, 0);
                movement *= Time.deltaTime;
				PlayerScript.distance += -movement.y;

                transform.Translate(movement);
            }
            else if (isLinkedToCamera && Camera.main.WorldToScreenPoint(transform.position).y > Camera.main.pixelHeight / 2 && rigidbody2D.velocity.y > 0)
		    {
                if (!firstUp)
                {
                    firstUp = true;
                    firstPos = transform.position;
				}

				height += (speed.y*Time.deltaTime);

                speed = new Vector2(0, rigidbody2D.velocity.y);

                transform.position = new Vector3(transform.position.x, firstPos.y, transform.position.z);
		    }
            else if (isLinkedToCamera && rigidbody2D.velocity.y <= 0)
            {
                speed = new Vector2(0, 0);
                firstUp = false;
            }

            if (isLooping)
            {
                Transform firstChild = backgroundPart.FirstOrDefault();

                if (firstChild != null)
                {
					if (isTransition)
					{
						Vector3 dirNorm = direction.normalized;
						
						if (dirNorm.y == -1 && firstChild.position.y < Camera.main.transform.position.y && !firstChild.renderer.IsVisibleFrom(Camera.main))
						{
							if (firstChild.Equals(background))
							{
								if (fillScreen)
								{
									isBackTrans = false;
								}
								else if (alignLeft)
								{
									isLeftTrans = false;
								}
								else if (alignRight)
								{
									isRightTrans = false;
								}

								if (!isBackTrans && !isLeftTrans && !isRightTrans)
									isTransition = false;

								backgroundPart.RemoveAt(0);
								Destroy (background.gameObject);
							}
							else if (!firstChild.Equals(transition))
							{
								Transform lastChild = backgroundPart.LastOrDefault();
								Vector3 lastPosition = lastChild.transform.position;
								Vector3 lastSize = lastChild.renderer.bounds.size;
								LoopLast(firstChild, new Vector3(firstChild.position.x, lastPosition.y + lastSize.y, firstChild.position.z), lastChild);
							}
							else if (firstChild.Equals(transition))
							{
								backgroundPart.RemoveAt (0);
								Destroy(firstChild.gameObject);
							}
						}
					}
					else
					{
	                    Vector3 dirNorm = direction.normalized;

	                    if (dirNorm.y == -1 && firstChild.position.y < Camera.main.transform.position.y && !firstChild.renderer.IsVisibleFrom(Camera.main))
	                    {
	                        Transform lastChild = backgroundPart.LastOrDefault();
	                        Vector3 lastPosition = lastChild.transform.position;
	                        Loop(firstChild, new Vector3(firstChild.position.x, lastPosition.y + firstChild.renderer.bounds.size.y, firstChild.position.z));
	                    }
					}
                }
            }
		}
	}

	public static int getHeight()
	{
		return (int)height;
	}

	private void LoopLast(Transform firstChild, Vector3 pos, Transform lastChild)
	{
		Transform newOne = (Transform)Instantiate(lastChild);
		newOne.parent = transform;

		newOne.position = pos;

		backgroundPart.RemoveAt(0);
		Destroy (firstChild.gameObject);
		backgroundPart.Add (newOne);
	}

    private void Loop(Transform firstChild, Vector3 pos)
    {
        firstChild.position = pos;

        backgroundPart.RemoveAt(0);
        backgroundPart.Add (firstChild);
    }
}
