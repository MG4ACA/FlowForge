# FlowForge: Phase 3 (The Visual Canvas) - Study Guide

This document summarizes the React Frontend concepts and boilerplate we implemented in Phase 3.

## 1. React Flow Architecture
React Flow uses two fundamental concepts:
1. **Nodes**: The physical blocks on the screen.
2. **Edges**: The lines connecting the nodes.

React operates on **Unidirectional Data Flow**. If a user drags a node or connects a wire, React Flow fires a warning (e.g., `onNodesChange` or `onConnect`). You must use a Lambda function to catch that warning and physically save the new data into React State (`useState`). If you don't save it to State, React throws the change away!

## 2. Custom Nodes & Handles (The Plumbing)
To use custom HTML cards instead of boring white boxes, you build a standard React Component and map it in a dictionary (e.g., `const nodeTypes = { email: EmailNode }`).

Inside your custom component, you must add `<Handle />` components. 
**Analogy:** Handles are the plumbing pipes sticking out of your node. Without them, you cannot connect any edges!
- `type="target"`: Incoming pipes (usually `Position.Top`).
- `type="source"`: Outgoing pipes (usually `Position.Bottom`).

```tsx
import { Handle, Position } from '@xyflow/react';

function EmailNode(props: any) {
  return (
    <div className='email-node-card'>
      <Handle type="target" position={Position.Top} />
      <h3>{props.data.label}</h3>
      <Handle type="source" position={Position.Bottom} />
    </div>
  );
}
export default EmailNode;
```

## 3. The Bouncer (CORS)
When the Frontend tries to send data (`fetch`) to a backend on a different port (e.g., Port 5173 talking to Port 5258), the browser's security Bouncer steps in and blocks it. This is called **Cross-Origin Resource Sharing (CORS)**.

To fix it, you must give the C# Backend a VIP Guest List to explicitly allow the React app to enter:
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
