
import { Background, Controls, Panel, ReactFlow, addEdge, applyEdgeChanges, applyNodeChanges, type Connection, type Edge, type EdgeChange, type Node, type NodeChange } from "@xyflow/react";
import '@xyflow/react/dist/style.css';
import { useState } from 'react';
import './App.css';
import EmailNode from './components/EmailNode';

const nodeTypes = {
  email: EmailNode
}

const initialNodes: Node[] = [{
  id: '1', position: { x: 250, y: 100 }, data: { label: 'Start' }
}, {
  id: '2', type: 'email', position: { x: 250, y: 250 }, data: { label: 'Send Email' }
}]
const initialEdges: Edge[] = []


function App() {
  const [savedWorkflowId, setSavedWorkflowId] = useState<string | null>(null);
  const [nodes, setNodes] = useState<Node[]>(initialNodes)
  const [edges, setEdges] = useState<Edge[]>(initialEdges)

  const onNodesChange = (changes: NodeChange[]) => setNodes((nds) => applyNodeChanges(changes, nds));
  const onEdgesChange = (changes: EdgeChange[]) => setEdges((eds) => applyEdgeChanges(changes, eds));
  const onConnect = (params: Connection) => setEdges((eds) => addEdge(params, eds));

  const onSave = async () => {
    const payload = {
      name: "Email Workflow",
      graphJson: JSON.stringify({ nodes, edges })
    };

    const response = await fetch("http://localhost:5258/api/workflows", {
      method: 'POST',
      headers: {
        'Content-type': 'application/json'
      },
      body: JSON.stringify(payload)
    });

    if (response.ok) {
      const data = await response.json(); // Open the envelope!
      setSavedWorkflowId(data.id);        // Save the tracking number to React State!
      alert('Workflow Saved with ID: ' + data.id);
    } else {
      alert('Failed to Save Workflow')
    }


  }

  const onRun = async () => {

    if (!savedWorkflowId) {
      alert("Please save the workflow first!");
    }

    const response = await fetch(`http://localhost:5129/api/workflows/${savedWorkflowId}/start`, {
      method: 'POST'

    });

    if (response.ok) {
      alert("Factory started! The workflow is running in the background!");
    } else {
      alert("Failed to start workflow")
    }
  };

  return <div style={{ width: '100vw', height: '100vh' }}>
    <ReactFlow nodes={nodes} onConnect={onConnect} edges={edges} nodeTypes={nodeTypes} onNodesChange={onNodesChange} onEdgesChange={onEdgesChange}>
      <Background />
      <Panel position="top-right">
        <div style={{ display: 'flex', flexDirection: 'row' }}>

          <button onClick={onSave} style={{ padding: '10px', background: 'blue', color: 'white', borderRadius: '5px' }}>
            Save Workflow
          </button>
          <button onClick={onRun} style={{ padding: '10px', margin: '10px', background: 'green', color: 'white', borderRadius: '5px' }}>Run Workflow</button>
        </div>
      </Panel>
      <Controls />
    </ReactFlow>
  </div>

}

export default App
