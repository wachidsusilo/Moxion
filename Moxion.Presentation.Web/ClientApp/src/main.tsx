import {StrictMode} from 'react'
import {createRoot} from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import {ClickEventProvider} from "./hooks/UseClickEvent.tsx";
import {MotionSimulationProvider} from "./hooks/UseMotionSimulation.tsx";

createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <ClickEventProvider>
            <MotionSimulationProvider>
                <App/>
            </MotionSimulationProvider>
        </ClickEventProvider>
    </StrictMode>,
)
