import {GetAvailableNamespaceResponse} from "../services/datacat-generated-client";

export const CURRENT_NAMESPACE_KEY = 'CURRENT_NAMESPACE';

export function getCurrentNamespaceFromLocalStorage(): GetAvailableNamespaceResponse {
    const raw = localStorage.getItem(CURRENT_NAMESPACE_KEY);
    if (raw) {
        try {
            return JSON.parse(raw) as GetAvailableNamespaceResponse;
        } catch {
            return {
                id: '00000000-0000-0000-0000-000000000000',
                name: 'datacat_system'
            } as GetAvailableNamespaceResponse;
        }
    }
    return {id: '00000000-0000-0000-0000-000000000000', name: 'datacat_system'} as GetAvailableNamespaceResponse;
}
