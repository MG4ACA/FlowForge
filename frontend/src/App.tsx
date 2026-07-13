
import { Background, Controls, ReactFlow, applyEdgeChanges, applyNodeChanges, type Edge, type EdgeChange, type Node, type NodeChange } from "@xyflow/react";
import '@xyflow/react/dist/style.css';
import { useState } from 'react';
import './App.css';
import EmailNode from './components/EmailNode';

const nodeTypes = {
  email: EmailNode
}

const initialNodes: Node[] = [{
  id: '1', position: {x:250, y:100}, data: {label: 'Start'}
}, {
  id: '2', type: 'email', position: {x:250, y:250}, data: {label: 'Send Email'}
}]
const initialEdges:Edge[] = []

function App() {
  const [nodes, setNodes] = useState<Node[]>(initialNodes)
  const [edges, setEdges] = useState<Edge[]>(initialEdges)

  const onNodesChange = (changes: NodeChange[]) => setNodes((nds) => applyNodeChanges(changes, nds));
const onEdgesChange = (changes: EdgeChange[]) => setEdges((eds) => applyEdgeChanges(changes, eds));


  return <div style={{width: '100vw', height: '100vh'}}>
    <ReactFlow nodes={nodes} edges={edges} nodeTypes={nodeTypes} onNodesChange={onNodesChange} onEdgesChange={onEdgesChange}>
    <Background />
    <Controls />
    </ReactFlow>
  </div>
  
}

export default App
