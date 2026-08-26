window.gcdsWrapperDocs = {
    preferredLanguage: function () {
        const languages = navigator.languages?.length ? navigator.languages : [navigator.language];
        const supportedPreference = languages.find(language => {
            const code = language?.toLowerCase();
            return code?.startsWith("fr") || code?.startsWith("en");
        });
        return supportedPreference?.toLowerCase().startsWith("fr") ? "fr" : "en";
    },
    setLanguage: function (language) {
        document.documentElement.lang = language;
    },
    copyText: function (text) {
        return navigator.clipboard.writeText(text);
    }
};
