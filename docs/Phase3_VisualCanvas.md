# FlowForge: Phase 3 (The Visual Canvas) - Study Guide

This document summarizes the React Frontend concepts and boilerplate we implemented in Phase 3.

## 1. React Flow Architecture
React Flow uses two fundamental concepts:
1. **Nodes**: The physical blocks on the screen.
2. **Edges**: The lines connecting the nodes.

React operates on **Unidirectional Data Flow**. If a user drags a node or connects a wire, React Flow fires a warning (e.g., `onNodesChange` or `onConnect`). You must use a Lambda function to catch that warning and physically save the new data into React State (`useState`). If you don't save it to State, React throws the change away!

## 2. The Foundation Boilerplate (Challenge 3.1)
Below is the exact enterprise boilerplate needed to render a basic, interactive React Flow canvas in `App.tsx`.

```tsx
import { useState } from 'react';
import { Background, Controls, ReactFlow, addEdge, applyEdgeChanges, applyNodeChanges, type Connection, type Edge, type EdgeChange, type Node, type NodeChange } from "@xyflow/react";
import '@xyflow/react/dist/style.css';

const initialNodes: Node[] = [{ id: '1', position: { x: 250, y: 100 }, data: { label: 'Start' } }];
const initialEdges: Edge[] = [];

function App() {
  const [nodes, setNodes] = useState<Node[]>(initialNodes);
  const [edges, setEdges] = useState<Edge[]>(initialEdges);

  const onNodesChange = (changes: NodeChange[]) => setNodes((nds) => applyNodeChanges(changes, nds));
  const onEdgesChange = (changes: EdgeChange[]) => setEdges((eds) => applyEdgeChanges(changes, eds));
  const onConnect = (params: Connection) => setEdges((eds) => addEdge(params, eds));

  return (
    <div style={{ width: '100vw', height: '100vh' }}>
      <ReactFlow nodes={nodes} edges={edges} onNodesChange={onNodesChange} onEdgesChange={onEdgesChange} onConnect={onConnect}>
        <Background />
        <Controls />
      </ReactFlow>
    </div>
  );
}
export default App;
```

## 3. Custom Nodes & Handles (The Plumbing)
To use custom HTML cards instead of boring white boxes, you build a standard React Component and map it in a dictionary (e.g., `const nodeTypes = { email: EmailNode }`).

Inside your custom component, you must add `<Handle />` components. 
**Analogy:** Handles are the plumbing pipes sticking out of your node. Without them, you cannot connect any edges!
- `type="target"`: Incoming pipes (usually `Position.Top`).
- `type="source"`: Outgoing pipes (usually `Position.Bottom`).

## 4. The Bouncer (CORS)
When the Frontend tries to send data (`fetch`) to a backend on a different port, the browser's security Bouncer steps in and blocks it. This is called **Cross-Origin Resource Sharing (CORS)**.

To fix it, you must give the C# Backend a VIP Guest List:
```csharp
// 1. Create the VIP List
builder.Services.AddCors(options => {
  options.AddDefaultPolicy(policy => {
    policy.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader();
  });
});

// 2. Tell the app to use it
app.UseCors();
```
