using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class RoomNodeSO : ScriptableObject
{
    [HideInInspector] public string roomNodeID;
    [HideInInspector] public List<string> parentRoomNodeIDList = new();
    [HideInInspector] public List<string> childRoomNodeIDList = new();
    [HideInInspector] public RoomNodeGraphSO roomNodeGraph;
    public RoomNodeTypeSO roomNodeType;
    [HideInInspector] public RoomNodeTypeListSO roomNodeTypeList;

    // Node layout values
    private const float RoomNodeWidth = 160f;

    private const float RoomNodeHeight = 75f;

    private const float CorridorNodeWidth = 100f;
    private const float CorridorNodeHeight = 75f;

    #region Editor Code

    // The following code should only be run in the Unity Editor
#if UNITY_EDITOR
    [HideInInspector] public GUIStyle style;
    [HideInInspector] public Rect rect;
    [HideInInspector] public bool isLeftClickDragging;
    [HideInInspector] public bool isSelected = false;

    public void Initialise(Rect nodeRect, RoomNodeGraphSO nodeGraph, RoomNodeTypeSO nodeType)
    {
        rect = nodeRect;
        roomNodeID = Guid.NewGuid().ToString();
        name = "RoomNode";
        roomNodeGraph = nodeGraph;
        roomNodeType = nodeType;

        // Load room node type list
        roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
    }

    /// Draw node with the provided style.
    public void Draw(GUIStyle nodeStyle)
    {
        style = nodeStyle;
        // Draw the node box using begin area
        GUILayout.BeginArea(rect, nodeStyle);

        // Start region to detect popup selection changes
        EditorGUI.BeginChangeCheck();

        if (roomNodeType.isCorridor)
        {
            rect.width = CorridorNodeWidth;
            rect.height = CorridorNodeHeight;
            GUI.contentColor = Color.white;
        }
        else if (roomNodeType.isChestRoom)
        {
            rect.width = RoomNodeWidth;
            rect.height = RoomNodeHeight;
            GUI.contentColor = Color.black;
        }
        else
        {
            rect.width = RoomNodeWidth;
            rect.height = RoomNodeHeight;
            GUI.contentColor = Color.white;
        }

        // If the room node has a parent or is an entrance, display a label. Otherwise, display a popup
        if (parentRoomNodeIDList.Count > 0 || childRoomNodeIDList.Count > 0 || roomNodeType.isEntrance)
            // Display the room node type label
            EditorGUILayout.LabelField(roomNodeType.roomNodeTypeName);
        else
        {
            // Display a popup using the RoomNodeType name values which can be selected from (default to the currently set RoomNodeType)
            var selected = roomNodeTypeList.list.FindIndex(type => type == roomNodeType);

            var selection = EditorGUILayout.Popup(selected, GetRoomNodeTypesToDisplay());

            roomNodeType = roomNodeTypeList.list[selection];
        }

        if (EditorGUI.EndChangeCheck())
            EditorUtility.SetDirty(this);

        GUILayout.EndArea();
    }

    /// Populate a string array with he room node type names that can be selected in the popup.
    private string[] GetRoomNodeTypesToDisplay()
    {
        var length = roomNodeTypeList.list.Count;
        var roomNamesArray = new string[length];

        for (int i = 0; i < length; i++)
        {
            if (!roomNodeTypeList.list[i].displayInNodeGraphEditor) continue;
            roomNamesArray[i] = roomNodeTypeList.list[i].roomNodeTypeName;
        }
        return roomNamesArray;
    }

    public void ProcessEvents(Event currentEvent)
    {
        switch (currentEvent.type)
        {
            case EventType.MouseDown:
                ProcessMouseDownEvent(currentEvent);
                break;

            case EventType.MouseUp:
                ProcessMouseUpEvent(currentEvent);
                break;

            case EventType.MouseDrag:
                ProcessMouseDragEvent(currentEvent);
                break;
        }
    }

    #region Event Processors

    private void ProcessMouseDownEvent(Event currentEvent)
    {
        if (currentEvent.button == 0) ProcessLeftClickDownEvent();
        else if (currentEvent.button == 1) ProcessRightClickDownEvent();

        void ProcessLeftClickDownEvent()
        {
            Selection.activeObject = this;

            // If shift or ctrl is held down, add to the selection
            if (currentEvent.shift || currentEvent.control)
                isSelected = !isSelected;
            else
            {
                // Select the node  and deselect all other nodes
                foreach (var roomNode in roomNodeGraph.roomNodeList.Where(
                             roomNode => roomNode != this && roomNode.isSelected))
                    roomNode.isSelected = false;
                isSelected = true;
            }
        }

        void ProcessRightClickDownEvent()
        {
            roomNodeGraph.SetNodeToDrawConnectionLineFrom(this, currentEvent.mousePosition);
        }
    }

    private void ProcessMouseUpEvent(Event currentEvent)
    {
        if (currentEvent.button == 0) ProcessLeftClickUpEvent();

        void ProcessLeftClickUpEvent()
        {
            if (isLeftClickDragging)
                isLeftClickDragging = false;
        }
    }

    private void ProcessMouseDragEvent(Event currentEvent)
    {
        if (currentEvent.button == 0) ProcessLeftClickDragEvent();

        void ProcessLeftClickDragEvent()
        {
            isLeftClickDragging = true;

            DragNode(currentEvent.delta);
            GUI.changed = true;
        }
    }

    #endregion Event Processors

    public void DragNode(Vector2 currentEventDelta)
    {
        rect.position += currentEventDelta;
        EditorUtility.SetDirty(this);
    }

    public bool AddChildRoomNodeIDToRoomNode(string childID)
    {
        if (!IsChildRoomNodeValid()) return false;
        childRoomNodeIDList.Add(childID);
        return true;

        bool IsChildRoomNodeValid()
        {
            var isBossNodeAlreadyConnected = false;
            // Check if there is an existing boss room node connected in the node graph
            foreach (var roomNode in roomNodeGraph.roomNodeList)
                if (roomNode.roomNodeType.isBossRoom && roomNode.parentRoomNodeIDList.Count > 0)
                    isBossNodeAlreadyConnected = true;

            //If the child node is of type boss room and there is already a boss node connected, return false
            if (roomNodeGraph.GetRoomNode(childID).roomNodeType.isBossRoom && isBossNodeAlreadyConnected)
                return false;

            // If the child node is of type none then return false
            if (roomNodeGraph.GetRoomNode(childID).roomNodeType.isNone) return false;

            // If the node already has a child with this childID then return false
            if (childRoomNodeIDList.Contains(childID)) return false;

            // If this node's ID and the childID are the same then return false
            if (roomNodeID == childID) return false;

            // If this child's ID is already in the parent list of this node then return false
            if (parentRoomNodeIDList.Contains(childID)) return false;

            // If child node already has a parent then return false
            if (roomNodeGraph.GetRoomNode(childID).parentRoomNodeIDList.Count > 0) return false;

            // If the child and this node are both corridors, return false
            if (roomNodeType.isCorridor && roomNodeGraph.GetRoomNode(childID).roomNodeType.isCorridor)
                return false;

            // If the child room and this node are both not corridors, return false
            if (!roomNodeType.isCorridor && !roomNodeGraph.GetRoomNode(childID).roomNodeType.isCorridor)
                return false;

            // If adding a corridor check that this node hasn't exceeded the max number of child corridors
            if (roomNodeGraph.GetRoomNode(childID).roomNodeType.isCorridor &&
                childRoomNodeIDList.Count >= Settings.maxChildCorridors)
                return false;

            // If the child room is an entrance return false - the entrance is always the root node
            if (roomNodeGraph.GetRoomNode(childID).roomNodeType.isEntrance) return false;

            // If adding the child room to a corridor, check that the corridor doesn't already have a child room
            if (!roomNodeGraph.GetRoomNode(childID).roomNodeType.isCorridor && childRoomNodeIDList.Count > 0)
                return false;

            return true;
        }
    }

    public bool AddParentRoomNodeIDToRoomNode(string parentID)
    {
        parentRoomNodeIDList.Add(parentID);
        return true;
    }

    public bool RemoveChildRoomNodeIDFromRoomNode(string childID)
    {
        if (!childRoomNodeIDList.Contains(childID)) return false;
        childRoomNodeIDList.Remove(childID);
        return true;
    }

    public bool RemoveParentRoomNodeIDFromRoomNode(string parentID)
    {
        if (!parentRoomNodeIDList.Contains(parentID)) return false;
        parentRoomNodeIDList.Remove(parentID);
        return true; ;
    }

#endif

    #endregion Editor Code
}