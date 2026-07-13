# FlowForge: Phase 3 The Visual Canvas

This document serves as a study guide and boilerplate reference for building the drag-and-drop UI using React Flow (`@xyflow/react`).

## 1. React Flow Architecture

React Flow is an industry-standard library for building node-based graphical interfaces. It operates on two fundamental concepts:

1. **Nodes**: The physical blocks you see on the screen (e.g., an "Email" block or a "Wait" block). Each node has an `id`, a `position` (X/Y coordinates), and `data` (the label or configuration of the block).
2. **Edges**: The lines connecting the nodes. An edge simply points from a `source` node ID to a `target` node ID.

To render a canvas, you must provide the `<ReactFlow>` component with an array of Nodes and an array of Edges. 

*Critical Rule:* The `<ReactFlow>` component MUST be placed inside a container `<div>` that has a defined width and height (like `100vw` and `100vh`). If the container has no height, the canvas will be completely invisible!

---

## 2. The Foundation Boilerplate (Challenge 3.1)

Below is the exact enterprise boilerplate needed to render a basic, interactive React Flow canvas. 

You can use this code in `App.tsx` to get your first visual grid on the screen. It includes two hardcoded nodes connected by a single edge to prove the canvas works.

```tsx
import { useState } from 'react';
import {
  ReactFlow,
  Controls,
  Background,
  applyNodeChanges,
  applyEdgeChanges,
  NodeChange,
  EdgeChange,
  Node,
  Edge
} from '@xyflow/react';

// CRITICAL: You must import the CSS, otherwise the canvas is invisible!
import '@xyflow/react/dist/style.css';

// 1. Define initial hardcoded Nodes
const initialNodes: Node[] = [
  { id: '1', position: { x: 250, y: 100 }, data: { label: 'Start Workflow' } },
  { id: '2', position: { x: 250, y: 250 }, data: { label: 'Send Email' } }
];

// 2. Define initial hardcoded Edges (connecting node 1 to node 2)
const initialEdges: Edge[] = [
  { id: 'e1-2', source: '1', target: '2' }
];

function App() {
  // We store the nodes and edges in React state so they can be updated when dragged
  const [nodes, setNodes] = useState<Node[]>(initialNodes);
  const [edges, setEdges] = useState<Edge[]>(initialEdges);

  // These functions tell React Flow how to update state when a user drags a node around
  const onNodesChange = (changes: NodeChange[]) => setNodes((nds) => applyNodeChanges(changes, nds));
  const onEdgesChange = (changes: EdgeChange[]) => setEdges((eds) => applyEdgeChanges(changes, eds));

  return (
    // The container MUST have width/height defined
    <div style={{ width: '100vw', height: '100vh' }}>
      <ReactFlow
        nodes={nodes}
        edges={edges}
        onNodesChange={onNodesChange}
        onEdgesChange={onEdgesChange}
        fitView // Automatically zooms the camera to fit all nodes on screen
      >
        {/* These add the grid background and the zoom controls in the corner */}
        <Background />
        <Controls />
      </ReactFlow>
    </div>
  );
}

export default App;
```
