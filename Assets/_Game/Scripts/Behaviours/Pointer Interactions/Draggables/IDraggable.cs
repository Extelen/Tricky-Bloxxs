using UnityEngine;

public interface IDraggable
{
    public void OnDragStart(Vector2 worldPosition);
    public void OnDragMove(Vector2 worldPosition);
    public void OnDragRelease(Vector2 worldPosition);
}