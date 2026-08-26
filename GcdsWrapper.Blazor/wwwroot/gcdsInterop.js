const stylesheetId = "gcds-components-stylesheet";
const moduleId = "gcds-components-module";

export function ensureAssets(cdnBase) {
    if (!document.getElementById(stylesheetId)) {
        const link = document.createElement("link");
        link.id = stylesheetId;
        link.rel = "stylesheet";
        link.href = `${cdnBase}/gcds.css`;
        document.head.appendChild(link);
    }

    if (!document.getElementById(moduleId)) {
        const script = document.createElement("script");
        script.id = moduleId;
        script.type = "module";
        script.src = `${cdnBase}/gcds.esm.js`;
        document.head.appendChild(script);
    }
}

function detailValue(event) {
    return event.detail?.value ?? event.detail ?? event.target?.value ?? null;
}

export function listen(element, eventName, dotNet, methodName) {
    const handler = event => dotNet.invokeMethodAsync(methodName, detailValue(event));
    element.addEventListener(eventName, handler);
    return { dispose: () => element.removeEventListener(eventName, handler) };
}

export function listenMany(element, eventNames, dotNet) {
    const registrations = eventNames.map(eventName => {
        const handler = event => dotNet.invokeMethodAsync(
            "HandleGcdsEvent",
            eventName,
            eventName === "gcdsInput" || eventName === "gcdsChange" ? detailValue(event) : null);
        element.addEventListener(eventName, handler);
        return [eventName, handler];
    });

    return {
        dispose: () => registrations.forEach(([eventName, handler]) => element.removeEventListener(eventName, handler))
    };
}

export function listenEvents(element, eventNames, dotNet) {
    const registrations = eventNames.map(eventName => {
        const handler = event => dotNet.invokeMethodAsync(
            "HandleGcdsEvent",
            eventName,
            event.detail === undefined ? null : JSON.stringify(event.detail));
        element.addEventListener(eventName, handler);
        return [eventName, handler];
    });

    return {
        dispose: () => registrations.forEach(([eventName, handler]) =>
            element.removeEventListener(eventName, handler))
    };
}
