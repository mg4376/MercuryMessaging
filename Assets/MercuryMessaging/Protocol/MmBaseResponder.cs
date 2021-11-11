// Copyright (c) 2017-2019, Columbia University
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met: 
// 
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer.
//  * Redistributions in binary form must reproduce the above copyright
//    notice, this list of conditions and the following disclaimer in the
//    documentation and/or other materials provided with the distribution.
//  * Neither the name of Columbia University nor the names of its
//    contributors may be used to endorse or promote products derived from
//    this software without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
// POSSIBILITY OF SUCH DAMAGE. 
//  
// =============================================================
// Authors: 
// Carmine Elvezio, Mengu Sukan, Samuel Silverman, Steven Feiner
// =============================================================
//  
//  
using System;
using System.Collections.Generic;
using MercuryMessaging.Task;
using UnityEngine;

namespace MercuryMessaging
{
    /// <summary>
    /// MmResponder that implements a switch handling
    /// the framework-provided MmMethods.
    /// </summary>
	public class MmBaseResponder : MmResponder
    {
        Dictionary<MmMethod, Action<MmMessage>> MmFuncDict = new Dictionary<MmMethod, Action<MmMessage>>();

        /// <summary>
        /// Awake gets the MmRelayNode, if one is present.
        /// Also calls the post-awake callback.
        /// </summary>
        public override void Awake()
        {
            MmLogger.LogFramework(gameObject.name + ": Base Responder Awake & Adding Base Functionality to Dict");

            MmFuncDict.Add(MmMethod.NoOp, delegate(MmMessage msg) { ExecuteNoOp(msg); });
            MmFuncDict.Add(MmMethod.SetActive, delegate(MmMessage msg) { ExecuteSetActive(msg); });
            MmFuncDict.Add(MmMethod.Refresh, delegate (MmMessage msg) { ExecuteRefresh(msg); });
            MmFuncDict.Add(MmMethod.Initialize, delegate (MmMessage msg) { ExecuteInitalize(msg); });
            MmFuncDict.Add(MmMethod.Switch, delegate (MmMessage msg) { ExecuteSwitch(msg); });
            MmFuncDict.Add(MmMethod.Complete, delegate (MmMessage msg) { ExecuteComplete(msg); });
            MmFuncDict.Add(MmMethod.TaskInfo, delegate (MmMessage msg) { ExecuteTaskInfo(msg); });
            MmFuncDict.Add(MmMethod.Message, delegate (MmMessage msg) { ExecuteMessage(msg); });
            MmFuncDict.Add(MmMethod.MessageBool, delegate (MmMessage msg) { ExecuteMessageBool(msg); });
            MmFuncDict.Add(MmMethod.MessageByteArray, delegate (MmMessage msg) { ExecuteMessageByteArray(msg); });
            MmFuncDict.Add(MmMethod.MessageFloat, delegate (MmMessage msg) { ExecuteMessageFloat(msg); });
            MmFuncDict.Add(MmMethod.MessageInt, delegate (MmMessage msg) { ExecuteMessageInt(msg); });
            MmFuncDict.Add(MmMethod.MessageSerializable, delegate (MmMessage msg) { ExecuteMessageSerializable(msg); });
            MmFuncDict.Add(MmMethod.MessageString, delegate (MmMessage msg) { ExecuteTaskInfo(msg); });
            MmFuncDict.Add(MmMethod.MessageTransform, delegate (MmMessage msg) { ExecuteMessageTransform(msg); });
            MmFuncDict.Add(MmMethod.MessageTransformList, delegate (MmMessage msg) { ExecuteMessageTransformList(msg); });
            MmFuncDict.Add(MmMethod.MessageVector3, delegate (MmMessage msg) { ExecuteMessageVector3(msg); });
            MmFuncDict.Add(MmMethod.MessageVector4, delegate (MmMessage msg) { ExecuteMessageVector4(msg); });
            MmFuncDict.Add(MmMethod.MessageGameObject, delegate (MmMessage msg) { ExecuteMessageGameObject(msg); });
        }

        protected void AddMethod(MmMethod myType, Action<MmMessage> myMethod)
        {
            MmFuncDict.Add(myType, myMethod);
        }

        /// <summary>
        /// Invoke an MmMethod. 
        /// Implements a switch that handles the different MmMethods
        /// defined by default set in MmMethod <see cref="MmMethod"/>
        /// </summary>
        /// <param name="msgType">Type of message. This specifies
        /// the type of the payload. This is important in 
        /// networked scenarios, when proper deseriaization into 
        /// the correct type requires knowing what was 
        /// used to serialize the object originally. <see cref="MmMessageType"/>
        /// </param>
        /// <param name="msg">The message to send.<see cref="MmMessage"/></param>
        public override void MmInvoke(MmMessage msg)
        {
            var type = msg.MmMethod;

            if (MmFuncDict.ContainsKey(type))
            {
                MmFuncDict[type].Invoke(msg);
            }
            else
            {
                Debug.Log(msg.MmMethod.ToString());
                throw new ArgumentOutOfRangeException();
            }
        }

        public virtual void ExecuteNoOp(MmMessage msg)
        {

        }

        public virtual void ExecuteSetActive(MmMessage msg)
        {
            var messageBool = (MmMessageBool)msg;
            SetActive(messageBool.value);
        }

        public virtual void ExecuteRefresh(MmMessage msg)
        {
            var messageTransform = (MmMessageTransformList)msg;
            Refresh(messageTransform.transforms);
        }

        public virtual void ExecuteInitalize(MmMessage msg)
        {
            Initialize();
        }

        public virtual void ExecuteSwitch(MmMessage msg)
        {
            var messageString = (MmMessageString)msg;
            Switch(messageString.value);
        }

        public virtual void ExecuteComplete(MmMessage msg)
        {
            var messageCompleteBool = (MmMessageBool)msg;
            Complete(messageCompleteBool.value);
        }

        public virtual void ExecuteTaskInfo(MmMessage msg)
        {
            var messageSerializable = (MmMessageSerializable)msg;
            ApplyTaskInfo(messageSerializable.value);
        }

        public virtual void ExecuteMessage(MmMessage msg)
        {
            ReceivedMessage(msg);
        }

        public virtual void ExecuteMessageBool(MmMessage msg)
        {
            ReceivedMessage((MmMessageBool)msg);
        }

        public virtual void ExecuteMessageByteArray(MmMessage msg)
        {
            ReceivedMessage((MmMessageByteArray)msg);
        }

        public virtual void ExecuteMessageFloat(MmMessage msg)
        {
            ReceivedMessage((MmMessageFloat)msg);
        }

        public virtual void ExecuteMessageInt(MmMessage msg)
        {
            ReceivedMessage((MmMessageInt)msg);
        }

        public virtual void ExecuteMessageSerializable(MmMessage msg)
        {
            ReceivedMessage((MmMessageSerializable)msg);
        }

        public virtual void ExecuteMessageString(MmMessage msg)
        {
            ReceivedMessage((MmMessageString)msg);
        }

        public virtual void ExecuteMessageTransform(MmMessage msg)
        {
            ReceivedMessage((MmMessageTransform)msg);
        }

        public virtual void ExecuteMessageTransformList(MmMessage msg)
        {
            ReceivedMessage((MmMessageTransformList)msg);
        }

        public virtual void ExecuteMessageVector3(MmMessage msg)
        {
            ReceivedMessage((MmMessageVector3)msg);
        }

        public virtual void ExecuteMessageVector4(MmMessage msg)
        {
            ReceivedMessage((MmMessageVector4)msg);
        }

        public virtual void ExecuteMessageGameObject(MmMessage msg)
        {
            ReceivedMessage((MmMessageGameObject)msg);
        }




        #region Base Message Handlers
        /// <summary>
        /// Handle MmMethod: SetActive
        /// </summary>
        /// <param name="active">Value of active state.</param>
        public virtual void SetActive(bool active)
        {
            gameObject.SetActive(active);

            MmLogger.LogResponder("SetActive(" + active + ") called on " + gameObject.name);
        }

        /// <summary>
        /// Handle MmMethod: Initialize
        /// Initialize allows you to provide additional initialization logic
        /// in-between calls to Monobehavior provided Awake() and Start() calls.
        /// </summary>
        public virtual void Initialize()
        {
            MmLogger.LogResponder("Initialize called on " + gameObject.name);
        }

        /// <summary>
        /// Handle MmMethod: Refresh
        /// </summary>
        /// <param name="transformList">List of transforms needed in refreshing an MmResponder.</param>
        public virtual void Refresh(List<MmTransform> transformList)
        {
            MmLogger.LogResponder("Refresh called on " + gameObject.name);
        }

        /// <summary>
        /// Handle MmMethod: Switch
        /// </summary>
        /// <param name="iName">Name of value in which to active.</param>
        protected virtual void Switch(string iName)
        {
        }

        /// <summary>
        /// Handle MmMethod: Switch
        /// </summary>
        /// <param name="active">Can be used to indicate active state 
        /// of object that triggered complete message</param>
        protected virtual void Complete(bool active)
        {
        }

        /// <summary>
        /// Handle MmMethod: TaskInfo
        /// Given a IMmSerializable, extract TaskInfo.
        /// </summary>
        /// <param name="serializableValue">Serializable class containing MmTask Info</param>
	    protected virtual void ApplyTaskInfo(IMmSerializable serializableValue)
	    {
	    }

	    /// <summary>
	    /// Handle MmMethod: Base MmMessage.
	    /// Override this to handle base Mercury Messages.
	    /// </summary>
	    /// <param name="message"><see cref="MmMessage"/></param>
	    protected virtual void ReceivedMessage(MmMessage message)
	    {
	    }

	    /// <summary>
	    /// Handle MmMethod: MessageBool.
	    /// Override this to handle Mercury's bool messages.
	    /// </summary>
	    /// <param name="message"><see cref="MmMessageBool"/></param>
	    protected virtual void ReceivedMessage(MmMessageBool message)
	    {
	    }

        /// <summary>
        /// Handle MmMethod: MessageByteArray.
        /// Override this to handle Mercury's byte array messages.
        /// </summary>
        /// <param name="message"><see cref="MmMessageByteArray"/></param>
        protected virtual void ReceivedMessage(MmMessageByteArray message)
	    {
	    }

        /// <summary>
        /// Handle MmMethod: MessageFloat.
        /// Override this to handle Mercury's float messages.
        /// </summary>
        /// <param name="message"><see cref="MmMessageFloat"/></param>
        protected virtual void ReceivedMessage(MmMessageFloat message)
	    {
	    }

	    /// <summary>
	    /// Handle MmMethod: MessageInt.
	    /// Override this to handle Mercury's int messages.
	    /// </summary>
	    /// <param name="message"><see cref="MmMessageInt"/></param>
	    protected virtual void ReceivedMessage(MmMessageInt message)
	    {
	    }

	    /// <summary>
	    /// Handle MmMethod: MessageSerializable.
	    /// Override this to handle Mercury's serializable messages.
	    /// </summary>
	    /// <param name="message"><see cref="MmMessageSerializable"/></param>
	    protected virtual void ReceivedMessage(MmMessageSerializable message)
	    {
	    }

        /// <summary>
        /// Handle MmMethod: MessageString
        /// Override this to handle Mercury's string messages.
        /// </summary>
        /// <param name="message"><see cref="MmMessageString"/></param>
		protected virtual void ReceivedMessage(MmMessageString message)
		{
		}

	    /// <summary>
	    /// Handle MmMethod: MessageTransform
	    /// Override this to handle Mercury's transform messages.
	    /// </summary>
	    /// <param name="message"><see cref="MmMessageTransform"/></param>
	    protected virtual void ReceivedMessage(MmMessageTransform message)
	    {
	    }

	    /// <summary>
	    /// Handle MmMethod: MessageTransformList
	    /// Override this to handle Mercury's transform list messages.
	    /// </summary>
	    /// <param name="message"><see cref="MmMessageTransformList"/></param>
	    protected virtual void ReceivedMessage(MmMessageTransformList message)
	    {
	    }

        /// <summary>
        /// Handle MmMethod: MessageVector3
        /// Override this to handle Mercury's Vector3 messages.
        /// </summary>
        /// <param name="message"><see cref="MmMessageVector3"/></param>
        protected virtual void ReceivedMessage(MmMessageVector3 message)
	    {
	    }

	    /// <summary>
	    /// Handle MmMethod: MessageVector4
	    /// Override this to handle Mercury's Vector4 messages.
	    /// </summary>
	    /// <param name="message"><see cref="MmMessageVector4"/></param>
	    protected virtual void ReceivedMessage(MmMessageVector4 message)
	    {
	    }

        /// <summary>
        /// Implementation of IMmResponder's GetRelayNode.
        /// </summary>
        /// <returns>Returns MmRelayNode if one attached to GameObject, 
        /// Otherwise returns NULL.
        /// </returns>
	    public override MmRelayNode GetRelayNode()
		{
			return GetComponent<MmRelayNode>();
		}

        #endregion
    }
}