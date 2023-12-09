using UnityEngine;
using UnityEngine.Video;
public class VideoPlayerController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject canvas;

    void Start()
    {
        if (videoPlayer != null)
        {
            // loopPointReached �̺�Ʈ�� OnVideoEnd �Լ��� ����մϴ�.
            videoPlayer.loopPointReached += OnVideoEnd;
        }
        else
        {
            Debug.LogError("Video Player�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }
    void Update()
    {
        // �ƹ� Ű�� ������ �� �������� �����մϴ�.
        if (Input.anyKeyDown)
        {
            StopVideo();
        }
    }
    void StopVideo()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
            canvas.SetActive(true);
            Debug.Log("�������� �����մϴ�.");
        }
    }

    // ������ ������ �� ȣ��� �Լ�
    void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("������ �������ϴ�.");
        canvas.SetActive(true);
    }

    // ����� ���� �Լ� ����
    void CallYourFunction()
    {
        Debug.Log("Video�� ������ ȣ��Ǵ� �Լ��Դϴ�.");
    }
}