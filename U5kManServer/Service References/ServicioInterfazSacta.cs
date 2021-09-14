﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

// 
// This source code was auto-generated by wsdl, Version=4.0.30319.17929.
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Web.Services.WebServiceBindingAttribute(Name="ServicioInterfazSactaSoap", Namespace="http://CD40.es/")]
public partial class ServicioInterfazSacta : System.Web.Services.Protocols.SoapHttpClientProtocol {
    
    private System.Threading.SendOrPostCallback GetEstadoSactaOperationCompleted;
    
    private System.Threading.SendOrPostCallback StartSactaOperationCompleted;
    
    private System.Threading.SendOrPostCallback EndSactaOperationCompleted;
    
    private System.Threading.SendOrPostCallback SactaConfGetOperationCompleted;
    
    private System.Threading.SendOrPostCallback SactaConfSetOperationCompleted;
    
    private System.Threading.SendOrPostCallback SactaSectorizationGetOperationCompleted;

    /// <remarks/>
    public ServicioInterfazSacta(string ipServer)
    {
        // this.Url = "http://localhost:51277/InterfazSacta/ServicioInterfazSacta.asmx";
        //this.Url = String.Format("http://{0}/nucleodf/u5kcfg/InterfazSacta/ServicioInterfazSacta.asmx", ipServer/*U5kManServer.Properties.u5kManServer.Default.MySqlServer*/);
        var Settings = U5kManServer.Properties.u5kManServer.Default;
        this.Url = $"http://{ipServer}/{Settings.SoapServicesMain}/InterfazSacta/ServicioInterfazSacta.asmx";
    }

    /// <remarks/>
    public event GetEstadoSactaCompletedEventHandler GetEstadoSactaCompleted;
    
    /// <remarks/>
    public event StartSactaCompletedEventHandler StartSactaCompleted;
    
    /// <remarks/>
    public event EndSactaCompletedEventHandler EndSactaCompleted;
    
    /// <remarks/>
    public event SactaConfGetCompletedEventHandler SactaConfGetCompleted;
    
    /// <remarks/>
    public event SactaConfSetCompletedEventHandler SactaConfSetCompleted;
    
    /// <remarks/>
    public event SactaSectorizationGetCompletedEventHandler SactaSectorizationGetCompleted;
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://CD40.es/GetEstadoSacta", RequestNamespace="http://CD40.es/", ResponseNamespace="http://CD40.es/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public byte GetEstadoSacta() {
        object[] results = this.Invoke("GetEstadoSacta", new object[0]);
        return ((byte)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginGetEstadoSacta(System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("GetEstadoSacta", new object[0], callback, asyncState);
    }
    
    /// <remarks/>
    public byte EndGetEstadoSacta(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((byte)(results[0]));
    }
    
    /// <remarks/>
    public void GetEstadoSactaAsync() {
        this.GetEstadoSactaAsync(null);
    }
    
    /// <remarks/>
    public void GetEstadoSactaAsync(object userState) {
        if ((this.GetEstadoSactaOperationCompleted == null)) {
            this.GetEstadoSactaOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetEstadoSactaOperationCompleted);
        }
        this.InvokeAsync("GetEstadoSacta", new object[0], this.GetEstadoSactaOperationCompleted, userState);
    }
    
    private void OnGetEstadoSactaOperationCompleted(object arg) {
        if ((this.GetEstadoSactaCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.GetEstadoSactaCompleted(this, new GetEstadoSactaCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://CD40.es/StartSacta", RequestNamespace="http://CD40.es/", ResponseNamespace="http://CD40.es/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public void StartSacta() {
        this.Invoke("StartSacta", new object[0]);
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginStartSacta(System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("StartSacta", new object[0], callback, asyncState);
    }
    
    /// <remarks/>
    public void EndStartSacta(System.IAsyncResult asyncResult) {
        this.EndInvoke(asyncResult);
    }
    
    /// <remarks/>
    public void StartSactaAsync() {
        this.StartSactaAsync(null);
    }
    
    /// <remarks/>
    public void StartSactaAsync(object userState) {
        if ((this.StartSactaOperationCompleted == null)) {
            this.StartSactaOperationCompleted = new System.Threading.SendOrPostCallback(this.OnStartSactaOperationCompleted);
        }
        this.InvokeAsync("StartSacta", new object[0], this.StartSactaOperationCompleted, userState);
    }
    
    private void OnStartSactaOperationCompleted(object arg) {
        if ((this.StartSactaCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.StartSactaCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://CD40.es/EndSacta", RequestNamespace="http://CD40.es/", ResponseNamespace="http://CD40.es/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public void EndSacta() {
        this.Invoke("EndSacta", new object[0]);
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginEndSacta(System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("EndSacta", new object[0], callback, asyncState);
    }
    
    /// <remarks/>
    public void EndEndSacta(System.IAsyncResult asyncResult) {
        this.EndInvoke(asyncResult);
    }
    
    /// <remarks/>
    public void EndSactaAsync() {
        this.EndSactaAsync(null);
    }
    
    /// <remarks/>
    public void EndSactaAsync(object userState) {
        if ((this.EndSactaOperationCompleted == null)) {
            this.EndSactaOperationCompleted = new System.Threading.SendOrPostCallback(this.OnEndSactaOperationCompleted);
        }
        this.InvokeAsync("EndSacta", new object[0], this.EndSactaOperationCompleted, userState);
    }
    
    private void OnEndSactaOperationCompleted(object arg) {
        if ((this.EndSactaCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.EndSactaCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://CD40.es/SactaConfGet", RequestNamespace="http://CD40.es/", ResponseNamespace="http://CD40.es/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public string SactaConfGet() {
        object[] results = this.Invoke("SactaConfGet", new object[0]);
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginSactaConfGet(System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("SactaConfGet", new object[0], callback, asyncState);
    }
    
    /// <remarks/>
    public string EndSactaConfGet(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((string)(results[0]));
    }
    
    /// <remarks/>
    public void SactaConfGetAsync() {
        this.SactaConfGetAsync(null);
    }
    
    /// <remarks/>
    public void SactaConfGetAsync(object userState) {
        if ((this.SactaConfGetOperationCompleted == null)) {
            this.SactaConfGetOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSactaConfGetOperationCompleted);
        }
        this.InvokeAsync("SactaConfGet", new object[0], this.SactaConfGetOperationCompleted, userState);
    }
    
    private void OnSactaConfGetOperationCompleted(object arg) {
        if ((this.SactaConfGetCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.SactaConfGetCompleted(this, new SactaConfGetCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://CD40.es/SactaConfSet", RequestNamespace="http://CD40.es/", ResponseNamespace="http://CD40.es/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public bool SactaConfSet(string jcfg) {
        object[] results = this.Invoke("SactaConfSet", new object[] {
                    jcfg});
        return ((bool)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginSactaConfSet(string jcfg, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("SactaConfSet", new object[] {
                    jcfg}, callback, asyncState);
    }
    
    /// <remarks/>
    public bool EndSactaConfSet(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((bool)(results[0]));
    }
    
    /// <remarks/>
    public void SactaConfSetAsync(string jcfg) {
        this.SactaConfSetAsync(jcfg, null);
    }
    
    /// <remarks/>
    public void SactaConfSetAsync(string jcfg, object userState) {
        if ((this.SactaConfSetOperationCompleted == null)) {
            this.SactaConfSetOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSactaConfSetOperationCompleted);
        }
        this.InvokeAsync("SactaConfSet", new object[] {
                    jcfg}, this.SactaConfSetOperationCompleted, userState);
    }
    
    private void OnSactaConfSetOperationCompleted(object arg) {
        if ((this.SactaConfSetCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.SactaConfSetCompleted(this, new SactaConfSetCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://CD40.es/SactaSectorizationGet", RequestNamespace="http://CD40.es/", ResponseNamespace="http://CD40.es/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public void SactaSectorizationGet() {
        this.Invoke("SactaSectorizationGet", new object[0]);
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginSactaSectorizationGet(System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("SactaSectorizationGet", new object[0], callback, asyncState);
    }
    
    /// <remarks/>
    public void EndSactaSectorizationGet(System.IAsyncResult asyncResult) {
        this.EndInvoke(asyncResult);
    }
    
    /// <remarks/>
    public void SactaSectorizationGetAsync() {
        this.SactaSectorizationGetAsync(null);
    }
    
    /// <remarks/>
    public void SactaSectorizationGetAsync(object userState) {
        if ((this.SactaSectorizationGetOperationCompleted == null)) {
            this.SactaSectorizationGetOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSactaSectorizationGetOperationCompleted);
        }
        this.InvokeAsync("SactaSectorizationGet", new object[0], this.SactaSectorizationGetOperationCompleted, userState);
    }
    
    private void OnSactaSectorizationGetOperationCompleted(object arg) {
        if ((this.SactaSectorizationGetCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.SactaSectorizationGetCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    public new void CancelAsync(object userState) {
        base.CancelAsync(userState);
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
public delegate void GetEstadoSactaCompletedEventHandler(object sender, GetEstadoSactaCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class GetEstadoSactaCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal GetEstadoSactaCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
            base(exception, cancelled, userState) {
        this.results = results;
    }
    
    /// <remarks/>
    public byte Result {
        get {
            this.RaiseExceptionIfNecessary();
            return ((byte)(this.results[0]));
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
public delegate void StartSactaCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
public delegate void EndSactaCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
public delegate void SactaConfGetCompletedEventHandler(object sender, SactaConfGetCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class SactaConfGetCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal SactaConfGetCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
            base(exception, cancelled, userState) {
        this.results = results;
    }
    
    /// <remarks/>
    public string Result {
        get {
            this.RaiseExceptionIfNecessary();
            return ((string)(this.results[0]));
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
public delegate void SactaConfSetCompletedEventHandler(object sender, SactaConfSetCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class SactaConfSetCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal SactaConfSetCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
            base(exception, cancelled, userState) {
        this.results = results;
    }
    
    /// <remarks/>
    public bool Result {
        get {
            this.RaiseExceptionIfNecessary();
            return ((bool)(this.results[0]));
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.17929")]
public delegate void SactaSectorizationGetCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
