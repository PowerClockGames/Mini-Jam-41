using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Powerclockgames.Tools
{
    [AddComponentMenu("Tools/Visible Collider")]
    public class VisibleCollider : MonoBehaviour
    {
        public Color gizmoColor = Color.blue;

        private PolygonCollider2D[] thisColliders;
        private List<Vector2[]> pointsList;
        private List<Vector3> _tList;

        void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;

            DrawPolygonCollider();
            DrawBoxCollider();
        }

        private void DrawPolygonCollider()
        {
            pointsList = new List<Vector2[]>();
            _tList = new List<Vector3>();

            thisColliders = gameObject.GetComponents<PolygonCollider2D>();
            foreach (PolygonCollider2D c in thisColliders)
            {

                Vector2[] pointWithOffset = new Vector2[c.points.Length];

                for (int i = 0; i < c.points.Length; i++)
                {
                    pointWithOffset[i] = c.points[i] + c.offset;
                }

                pointsList.Add(pointWithOffset);
                _tList.Add(c.transform.position);
            }

            for (int j = 0; j < pointsList.Count; j++)
            {
                Vector2[] points = pointsList[j];
                Vector3 _t = _tList[j];

                for (int i = 0; i < points.Length - 1; i++)
                {
                    Gizmos.DrawLine(new Vector3(points[i].x + _t.x, points[i].y + _t.y), new Vector3(points[i + 1].x + _t.x, points[i + 1].y + _t.y));
                }

                Gizmos.DrawLine(new Vector3(points[points.Length - 1].x + _t.x, points[points.Length - 1].y + _t.y), new Vector3(points[0].x + _t.x, points[0].y + _t.y));
            }
        }

        private void DrawBoxCollider()
        {
            BoxCollider2D box2d = gameObject.GetComponent<BoxCollider2D>();
            BoxCollider box3d = gameObject.GetComponent<BoxCollider>();

            if (box2d)
            {
                Vector3 box2dVector = new Vector3(box2d.offset.x, box2d.offset.y, 0);
                Gizmos.DrawWireCube(box2d.transform.position + box2dVector, box2d.bounds.size);
            }
            else if (box3d)
            {
                Gizmos.DrawCube(transform.position, box3d.size);
            }
        }
    }
}
