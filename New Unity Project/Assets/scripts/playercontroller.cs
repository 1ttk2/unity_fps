using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playercontroller : MonoBehaviour
{
    public Transform viewPoint;//�J�����̈ʒu�I�u�W�F�N�g
    public float mouseSensitivity = 1f;//���_�ړ��̑��x
    private Vector2 mouseInput;//���[�U�[�̃}�E�X���͂��i�[
    private float verticalMouseInput;//y���̉�]���i�[�@��]�𐧌�����������
    private Camera cam;//�J����



    private Vector3 moveDir;//�v���C���[�̓��͂��i�[�i�ړ��j
    private Vector3 movement;//�i�ޕ������i�[����ϐ�
    private float activeMoveSpeed = 4;//���ۂ̈ړ����x


    private void Start()
    {
        //�ϐ��Ƀ��C���J�������i�[
        cam = Camera.main;
    }

    private void Update()
    {
        //���_�ړ��֐�
        PlayerRotate();

        //�ړ��֐�
        PlayerMove();
    }

    //Update�֐����Ă΂ꂽ��Ɏ��s�����
    private void LateUpdate()
    {
        //�J�������v���C���[�̎q�ɂ���̂ł͂Ȃ��A�X�N���v�g�ňʒu�����킹��
        cam.transform.position = viewPoint.position;
        cam.transform.rotation = viewPoint.rotation;
    }

    /// <summary>
    /// Player�̉���]�Əc�̎��_�ړ����s��
    /// </summary>
    public void PlayerRotate()
    {
        //�ϐ��Ƀ��[�U�[�̃}�E�X�̓������i�[
        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X") * mouseSensitivity,
            Input.GetAxisRaw("Mouse Y") * mouseSensitivity);

        //����]�𔽉f(transform.eulerAngles�̓I�C���[�p�Ƃ��Ă̊p�x���Ԃ����)
        transform.rotation = Quaternion.Euler
            (transform.eulerAngles.x,
            transform.eulerAngles.y + mouseInput.x, //�}�E�X��x���̓��͂𑫂�
            transform.eulerAngles.z);


        //�ϐ���y���̃}�E�X���͕��̐��l�𑫂�
        verticalMouseInput += mouseInput.y;

        //�ϐ��̐��l���ۂ߂�i�㉺�̎��_����j
        verticalMouseInput = Mathf.Clamp(verticalMouseInput, -60f, 60f);

        //�c�̎��_��]�𔽉f(-��t���Ȃ��Ə㉺���]���Ă��܂�)
        viewPoint.rotation = Quaternion.Euler
            (-verticalMouseInput,
            viewPoint.transform.rotation.eulerAngles.y,
            viewPoint.transform.rotation.eulerAngles.z);
    }



    /// <summary>
    /// Player�̈ړ�
    /// </summary>
    public void PlayerMove()
    {
        //�ϐ��̐����Ɛ����̓��͂��i�[����iwasd����̓��́j
        moveDir = new Vector3(Input.GetAxisRaw("Horizontal"),
            0, Input.GetAxisRaw("Vertical"));

        //Debug.Log(moveDir);�����p

        //�Q�[���I�u�W�F�N�g�̂�����x���ɓ��͂��ꂽ�l��������Ɛi�ޕ������o����
        movement = ((transform.forward * moveDir.z) + (transform.right * moveDir.x)).normalized;

        //���݈ʒu�ɐi�ޕ������ړ��X�s�[�h���t���[���ԕb���𑫂�
        transform.position += movement * activeMoveSpeed * Time.deltaTime;
    }
}