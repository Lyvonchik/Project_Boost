using UnityEngine;
using UnityEngine.SceneManagement;

public class Kapelka : MonoBehaviour
{
    [SerializeField] float rotationValue = 100f;
    [SerializeField] float powerValue = 100f;
    [SerializeField] AudioClip kapelkaSound, winSound, failSound;
    [SerializeField] ParticleSystem kapParticle, destroyParticle;
    [SerializeField] static int CurrentLvl = 0;

    new Rigidbody rigidbody;
    AudioSource audioSource;

    enum State { Alive, Dead, Complete }
    State state = State.Alive;

    bool collisionEnable = true;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        SuppButton();
        if (state == State.Alive)
        {
            PressSpase();
            Rotate();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || !collisionEnable) { return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;

            case "Finish":
                Finish();
                break;

            default:
                Rip();
                break;
        }
    }

    private void Rip()
    {
        state = State.Dead;
        kapParticle.Stop();
        destroyParticle.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(failSound);
        Invoke("Death", 1f);
    }

    private void Finish()
    {
        state = State.Complete;
        kapParticle.Stop();
        destroyParticle.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(winSound);
        Invoke("ToNextLvl", 1f);
    }

    private void Death()
    {
        SceneManager.LoadScene(CurrentLvl);
    }

    private void SuppButton()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ToNextLvl();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionEnable = !collisionEnable;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void ToNextLvl()
    {
        if (CurrentLvl == 3)
        {
            CurrentLvl = 0;
            SceneManager.LoadScene(0);
        }
        else
        {
            CurrentLvl = CurrentLvl + 1;
            SceneManager.LoadScene(CurrentLvl);
        }
    }

    private void PressSpase()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidbody.AddRelativeForce(Vector3.up * powerValue * Time.deltaTime);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(kapelkaSound);
            }
            if (!kapParticle.isPlaying)
            {
                kapParticle.Play();
            }
        }
        else
        {
            audioSource.Stop();
            kapParticle.Stop();
        }
    }

    private void Rotate()
    {
        rigidbody.angularVelocity = Vector3.zero;

        float rotationSpeed = rotationValue * Time.deltaTime;
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationSpeed);
        }
    }
}