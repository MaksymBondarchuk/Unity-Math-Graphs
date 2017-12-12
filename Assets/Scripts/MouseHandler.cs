using UnityEngine;
// ReSharper disable UnusedMember.Local

namespace Assets.Scripts
{
    // ReSharper disable once UnusedMember.Global
    public class MouseHandler : MonoBehaviour
    {
        private Vector3 RightClick { get; set; }

        private bool IsRightClicked { get; set; }

        private Vector3 MiddleClick { get; set; }

        private bool IsMiddleClicked { get; set; }

        private void Start()
        {
            Cursor.visible = true;
        }

        private void Update()
        {
            #region MoveCamera
            if (Input.GetMouseButtonDown(1))
            {
                RightClick = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
                IsRightClicked = true;
            }

            if (Input.GetMouseButtonUp(1))
            {
                IsRightClicked = false;
            }

            if (IsRightClicked)
                MoveRightPressed();

            if (Input.GetMouseButtonDown(2))
            {
                MiddleClick = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
                IsMiddleClicked = true;
            }

            if (Input.GetMouseButtonUp(2))
            {
                IsMiddleClicked = false;
            }

            if (IsMiddleClicked)
                MoveMiddlePressed();
            #endregion MoveCamera

            if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
                MoveCamera(1);
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
                MoveCamera(-1);
        }

        private void MoveCamera(int step)
        {
            transform.Translate(step > 0 ? Vector3.forward : Vector3.back);
        }

        private void MoveRightPressed()
        {
            var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
            transform.RotateAround(transform.position, new Vector3(1.0f, 0.0f, 0.0f), (RightClick.y - mousePosition.y) * Time.deltaTime * 5);
            transform.RotateAround(transform.position, new Vector3(0.0f, 1.0f, 0.0f), -(RightClick.x - mousePosition.x) * Time.deltaTime * 5);
            transform.RotateAround(transform.position, new Vector3(0.0f, 0.0f, 1.0f), (RightClick.z - mousePosition.z) * Time.deltaTime * 5);
            RightClick = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        }

        private void MoveMiddlePressed()
        {
            var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
            transform.Translate(new Vector3((MiddleClick.x - mousePosition.x) * Time.deltaTime, (MiddleClick.y - mousePosition.y) * Time.deltaTime, 0));
            MiddleClick = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        }
    }
}
