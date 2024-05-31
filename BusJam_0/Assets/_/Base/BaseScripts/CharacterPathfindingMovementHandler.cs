using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using V_AnimationSystem;
using CodeMonkey.Utils;

public class CharacterPathfindingMovementHandler : MonoBehaviour
{
    private Animator animator;
    private const float speed = 55f;
    private int currentPathIndex;
    private List<Vector3> pathVectorList;
    public CreateGrid createGrid;
    public CharacterController characterController;
    public GameManager gameManager;
    bool notAnymore = false;
    private void Start()
    {
        animator=GetComponent<Animator>();
        createGrid = GameObject.FindAnyObjectByType<CreateGrid>();
        gameManager = GameObject.FindAnyObjectByType<GameManager>();
        Debug.Log("bulundu " + createGrid);
        characterController = GetComponent<CharacterController>();
        CheckCharacterCanGo();
    }
    float timer = 0.05f;
    private void RotateTowards(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 35);
        }
    }
    bool finishMove = false;
    bool moveToCar = false;
    bool moveToReserve = false;
    private void MoveToPosition(GameObject transformPos, Vector3 targetPos)
    {
        float distanceToTarget = Vector3.Distance(transformPos.transform.position, targetPos);

        if (distanceToTarget > 0.1f) // Hedefe yaklaştığında durma kontrolü
        {
            Vector3 moveDir = (targetPos - transformPos.transform.position).normalized;
            transform.position += moveDir * 40 * Time.deltaTime;
            RotateTowards(moveDir);
        }
        else
        {
      
        }
    }
    private void Update()
    {

        //CheckCharacterCanGo();
        if (gameObject == CreateGrid.player && (characterController.characterColor != CharacterColor.Destroy) && notAnymore == false)
        {
            finishMove = true;


            if (Input.GetMouseButtonDown(0))
            {
                SetTargetPosition(new Vector3(45, 0, 45));
               
            }
        }
        if (finishMove)
        {
            HandleMovement();
        }
        if (moveToCar)
        {
            MoveToPosition(gameObject, new Vector3(59, 0, 118));

        }
        

        if (GameManager.can && timer >= 0)
        {
           // print("can");
            //CheckCharacterCanGo();
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                GameManager.can = false;
                timer = 0.05f;
            }

        }}
    public void CheckCharacterCanGo()
    {
        SetTargetPosition(new Vector3(45, 0, 45));
        if (pathVectorList != null)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
        }
    }
    private void HandleMovement()
    {


        if (pathVectorList != null)
        {
            Vector3 targetPosition = pathVectorList[currentPathIndex];
            targetPosition = new Vector3(targetPosition.x, 0, targetPosition.z);
            if (Vector3.Distance(transform.position, targetPosition) > 1f)
            {
                Vector3 moveDir = (targetPosition - transform.position).normalized;
                float distanceBefore = Vector3.Distance(transform.position, targetPosition);
                RotateTowards(moveDir);
                //animatedWalker.SetMoveVector(moveDir);
                transform.position = transform.position + moveDir * speed * Time.deltaTime;
                animator.SetBool("isWalking", true);
                //print(moveDir);
            }
            else
            {
                currentPathIndex++;
                if (currentPathIndex >= pathVectorList.Count)
                {
                    StopMoving();
                    animator.SetBool("isWalking", false);
                    finishMove = false;
                    //animatedWalker.SetMoveVector(Vector3.zero);
                    CheckCarAndGo();
                }
            }
        }
        else
        {
            //animatedWalker.SetMoveVector(Vector3.zero);
        }

    }
    
    public void CheckCarAndGo()
    {
        if (characterController.characterColor.ToString() == gameManager.CarCurrentColor().ToString())
        {
            //  gameManager.AddReserveCharacter(gameObject);
            
            gameManager.CheckReserveAndGo();
            Debug.Log("GIDEBILIR");
            moveToCar=true;
            gameManager.GoToCar(gameObject);
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("YEDEKTE");
            gameManager.AddReserveCharacter(gameObject);
        }
    }
    private void StopMoving()
    {
        pathVectorList = null; CreateGrid.player = null;notAnymore = true;
    }

    public Vector3 GetPosition()
    {
        //Debug.LogError("pozisyonu" + transform.position);
        return transform.position;
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        currentPathIndex = 0;
        //pathVectorList = Pathfinding.Instance.FindPath(GetPosition(), targetPosition);
        pathVectorList = createGrid.pathfinding.FindPath(GetPosition(), targetPosition);

        if (pathVectorList != null && pathVectorList.Count > 1)
        {
            pathVectorList.RemoveAt(0);
        }
    }
    public void WalkableClose()// close walkable
    {
        Vector3 position = GetPosition();
        createGrid.pathfinding.GetGrid().GetXY(position, out int x, out int y); //Debug.LogError(x + ", " + y);
        PathNode node = createGrid.pathfinding.GetNode(x, y);
        node.SetIsWalkable(false);
    }
    public void WalkableCloseInt(out int a, out int b)// close walkable
    {
        Vector3 position = GetPosition();
        createGrid.pathfinding.GetGrid().GetXY(position, out int x, out int y);

        a = x; b = y;
    }


}