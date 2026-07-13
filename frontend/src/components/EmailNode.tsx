import { Handle, Position } from '@xyflow/react';


function EmailNode(props: any) {
  return (
    <div className='email-node-card' style={{ padding: '10px', border: '1px solid black', borderRadius: '5px', background: 'white' }}>
      <Handle type='target' position={Position.Top}/>
      <div className='email-node-card__icon'>📧</div>
      <div className='email-node-card__content'>
        <h3>{props.data.label}</h3>
        <button style={{ display: 'block', marginTop: '10px' }}>Configure Email</button>
      </div>
      <Handle type='source' position={Position.Bottom}/>
    </div>
  );
}

export default EmailNode;
