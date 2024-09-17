using Interfaces;
using UnityEngine;

namespace Game.Level
{
    public class Walls : MonoBehaviour, IServisable
    {
        [SerializeField] private BoxCollider2D _leftWall;
        [SerializeField] private BoxCollider2D _rightWall;
        [SerializeField] private BoxCollider2D _topWall;
        [SerializeField] private BoxCollider2D _bottonWall;
        [SerializeField] private RectTransform _rightCanvasPanel;

        public Bounds Bounds { get; private set; }

        public void SetupWalls(Camera mainCamera)
        {
            var xAnchor = _rightCanvasPanel.anchorMin.x;
            var border = mainCamera.ViewportToWorldPoint(new Vector3(0f, 1f, 0f));
            var rightBorder = mainCamera.ViewportToWorldPoint(new Vector3(xAnchor, 1f, 0f));
            var scaleVector = new Vector2(Mathf.CeilToInt(mainCamera.orthographicSize * mainCamera.aspect * 2f), mainCamera.orthographicSize * 2f);

            var leftWallTr = _leftWall.transform;
            var leftWallScale = leftWallTr.localScale;
            leftWallTr.localScale = new Vector3(1f, leftWallScale.y * scaleVector.y, 1f);
            leftWallTr.position = new Vector3(border.x - _leftWall.size.x * 0.5f, 0f, 0f);

            var rightWallTr = _rightWall.transform;
            var rightWallScale = rightWallTr.localScale;
            rightWallTr.localScale = new Vector3(1f, rightWallScale.y * scaleVector.y, 1f);
            rightWallTr.position = new Vector3(rightBorder.x + _rightWall.size.x * 0.5f, 0f, 0f);

            var topWallTr = _topWall.transform;
            var topWallScale = topWallTr.localScale;
            topWallTr.localScale = new Vector3(topWallScale.x * scaleVector.x, 1f, 1f);
            topWallTr.position = new Vector3(0f, border.y + _topWall.size.y * 0.5f, 0f);

            var bottomWallTr = _bottonWall.transform;
            var bottomWallScale = bottomWallTr.localScale;
            bottomWallTr.localScale = new Vector3(bottomWallScale.x * scaleVector.x, 1f, 1f);
            bottomWallTr.transform.position = new Vector3(0f, -border.y - _bottonWall.size.y * 0.5f, 0f);

            Bounds = new Bounds(new Vector3((border.x + rightBorder.x) * 0.5f, 0f, 0f), new Vector3(-border.x + rightBorder.x, border.y * 2f, 0f));
        }
    }
}
