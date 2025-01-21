import {createContext, ReactNode, useContext, useEffect} from "react";

interface IClickEvent {
    registerClickConsumer(id: string, call: () => void, windowEvent?: boolean): void

    unregisterClickConsumer(id: string): void

    dispatchClick(...excludeIds: string[]): void
}

interface IClickEventCallback {
    id: string
    windowEvent: boolean

    invoke(): void
}

const callbacks: Array<IClickEventCallback> = []

const registerClickConsumer = (id: string, call: () => void, windowEvent: boolean = true) => {
    if (!callbacks.some(value => value.id === id)) {
        callbacks.push({id, invoke: call, windowEvent})
    }
}

const unregisterClickConsumer = (id: string) => {
    const idx = callbacks.findIndex(value => value.id === id)
    if (idx >= 0) {
        callbacks.splice(idx, 1)
    }
}

const dispatchClick = (...excludeIds: Array<string>) => {
    for (const callback of callbacks) {
        if (!excludeIds.some(id => id === callback.id)) {
            callback.invoke()
        }
    }
}

const ClickEventContext = createContext<IClickEvent>({
    registerClickConsumer,
    unregisterClickConsumer,
    dispatchClick
})

interface IClickEventProviderProps {
    children?: ReactNode
}

export function ClickEventProvider({children}: IClickEventProviderProps) {

    useEffect(() => {
        const onClick = () => {
            for (const callback of callbacks) {
                if (callback.windowEvent) {
                    callback.invoke()
                }
            }
        }

        window.addEventListener('click', onClick)
        return () => {
            window.removeEventListener('click', onClick)
        }
    }, [])

    return (
        <ClickEventContext.Provider value={{
            registerClickConsumer,
            unregisterClickConsumer,
            dispatchClick
        }}>
            {children}
        </ClickEventContext.Provider>
    )
}

export default function useClickEvent() {
    return useContext(ClickEventContext)
}
