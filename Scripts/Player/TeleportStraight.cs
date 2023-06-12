using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TeleportStraight : MonoBehaviour
{
    // Start is called before the first frame update

    //텔레포트를 표시할 UI
    public Transform teleportCircleUI;
    //선을 그릴 라인 렌더러
    LineRenderer lr;
    // 최초 텔레포트 UI
    Vector3 originScale = Vector3.one * 0.02f;
    // 위프 사용 여부
    public bool isWarp = false;
    // 뭐프에 걸리는 시간
    public float warpTime = 0.1f;
    // 사용하고 있는 포스트 프로세싱 볼륨
    public PostProcessVolume post;

    void Start()
    {
        //시작할 때 비활성화 한다.
        teleportCircleUI.gameObject.SetActive(false);
        // 라인 렌더러 컴호넌트 얻어오기
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ARAVRInput.GetDown(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.LTouch))
        {
            //라인 렌더러 컴호넌트 활성화;
            lr.enabled = true;
        }
        else if (ARAVRInput.GetUp(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.LTouch))
        {
            //라인 렌터러 비활성화
            lr.enabled = false;

            if (teleportCircleUI.gameObject.activeSelf)
            {
                //워프 기능 사용이 아닐때 순간이동 처리
                if (isWarp == false)
                {
                    GetComponent<CharacterController>().enabled = false;
                    //텔레포트 UI위치로 순간이동
                    transform.position = teleportCircleUI.position + Vector3.up;
                    GetComponent<CharacterController>().enabled = true;
                }
                else
                {
                    //위프 기능을 사용할때는 Warp() 코루틴 호출
                    StartCoroutine(Warp());
                }
            }
            // 텔레포트 UI 비활성화
            teleportCircleUI.gameObject.SetActive(false);
        }
        else if(ARAVRInput.Get(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.LTouch))
        {
            //텔레포드 UI 그리기
            //1. 왼쪽 컨트롤러 기준 Ray 생성
            Ray ray = new Ray(ARAVRInput.LHandPosition, ARAVRInput.LHandDirection);
            RaycastHit hitinfo;
            int layer = 1 << LayerMask.NameToLayer("Terrain");
            //2. Terrain만 Ray충돌 검출
            if(Physics.Raycast(ray, out hitinfo, 200, layer))
            {
                // 3. Ray가 부딪힌 지점에 라인 그리기
                lr.SetPosition(0, ray.origin);
                lr.SetPosition(1, hitinfo.point);
                //4. Ray가 부딪힌 지점에 텔레포트 UI 표시
                teleportCircleUI.gameObject.SetActive(true);
                teleportCircleUI.position = hitinfo.point;
                // 텔레포트 UI가 위로 누위 있도록 방향 설정
                teleportCircleUI.forward = hitinfo.normal;
                // 텔레포트 UI의 크기가 거리에 따라 보정되도록 설정
                teleportCircleUI.localScale = originScale * Mathf.Max(1, hitinfo.distance);
            }
            else
            {
                // Ray 출돌이 발생하니 잖으면 선이 Ray 방향으로 그러지도록 처리
                lr.SetPosition(0, ray.origin);
                lr.SetPosition(1, ray.origin + ARAVRInput.LHandDirection * 200);
                // 텔레포트는 UI 화면에서 비활성화
                teleportCircleUI.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator Warp()
    {
        //워프느낌을 표현할 모션블러
        MotionBlur blur;
        //워프 시작점 기억
        Vector3 pos = transform.position;
        //목적지
        Vector3 targetPos = teleportCircleUI.position + Vector3.up;
        //워프 시간 경과
        float currentTime = 0;
        //포스트 프로세싱에서 사용중인 프로파일에서 모션블러 얻어오기
        post.profile.TryGetSettings<MotionBlur>(out blur);
        //워프 시작전 블러 켜기
        blur.active = true;
        GetComponent<CharacterController>().enabled = false;

        while (currentTime < warpTime)
        {
            //경과 시간 흐르게 하기
            currentTime += Time.deltaTime;
            //워프의 시작점에서 도착점에 도착하기 위해 워프 시간동한 이동
            transform.position = Vector3.Lerp(pos, targetPos, currentTime / warpTime);
            //코두틴 대기
            yield return null;
        }
        // 텔레포트 UI 위치로 순간이동
        transform.position = teleportCircleUI.position + Vector3.up;
        // 캐릭터 컨트롤러 다시 켜기
        GetComponent<CharacterController>().enabled = true;
        // 포스트 효과 켜기
        blur.active = false;
    }
}
