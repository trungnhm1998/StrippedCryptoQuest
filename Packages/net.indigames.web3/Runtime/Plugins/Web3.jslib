mergeInto(LibraryManager.library, {
    /**
     * Open Web3 wallet and allow user to sign in
     * @param objectName Unity GameObject's name
     * @param callback Callback when finish successfully
     * @param fallback Fallback when failed
     */    
    SignIn: function (objectName, callback, fallback) {
        var parsedObjectName = UTF8ToString(objectName);
        var parsedCallback = UTF8ToString(callback);
        var parsedFallback = UTF8ToString(fallback);
        try {
            ethereum.request({ method: 'eth_requestAccounts'})
            .then((_) => {
                window.ethProvider.getSigner()
                .then((signer) => {
                    window.Web3Token.sign(msg => signer.signMessage(msg), '1d')
                    .then((result) => {
                        window.unityInstance.SendMessage(parsedObjectName, parsedCallback, result);
                    })
                    .catch((error) => {
                        window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
                    });
                })
                .catch((error) => {
                    window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
                });
            })
            .catch((error) => {
                window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
            });
        } catch (error) {
            window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
        }
    },

});