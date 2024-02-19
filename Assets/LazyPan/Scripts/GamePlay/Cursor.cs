using UnityEngine;

namespace LazyPan {
    public class Cursor : Singleton<Cursor> {
        public Transform cursor;
        public Vector3 hitPoint;
        private float cursorRayDeploy;
        private float cursorRayInterval = 0.1f;

        public void SetCursor() {
            cursorRayDeploy = cursorRayInterval;

            Texture2D cursorTexture = Loader.LoadAsset<Texture2D>(CommonAssetType.CURSOR);
            UnityEngine.Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);

            Data.Instance.OnLateUpdateEvent.AddListener(CursorUpdate);
        }

        public void SetAimCursor() {
            cursor = UI.Instance.Get("UI_Fight").Get<Transform>("UI_Cursor");
            cursor.gameObject.SetActive(true);
            Data.Instance.OnLateUpdateEvent.AddListener(CursorLateUpdate);
        }

        private void CursorUpdate() {
            if (cursorRayDeploy > 0) {
                cursorRayDeploy -= Time.deltaTime;
            } else {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit HitInfo, Mathf.Infinity, 1 << LayerMask.NameToLayer("Floor"))) {
                    hitPoint = HitInfo.point;
                }
                cursorRayDeploy = cursorRayInterval;
            }
        }

        private void CursorLateUpdate() {
            if (cursor != null) {
                cursor.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            }
        }

        public void ClearAimCursor() {
            cursor.gameObject.SetActive(false);
            Data.Instance.OnLateUpdateEvent.RemoveListener(CursorLateUpdate);
        }

        public void ClearCursor() {
            Data.Instance.OnLateUpdateEvent.RemoveListener(CursorUpdate);
        }
    }
}