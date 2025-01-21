import {DateTime} from "luxon"
import Axios, {AxiosRequestConfig} from "axios"

class Api {
    private _accessToken: string | null = null
    private _expiredAt: DateTime = DateTime.now()
    private readonly _baseUrl: string

    constructor(baseUrl: string) {
        this._baseUrl = baseUrl
    }

    private _globalConfig: AxiosRequestConfig = {
        validateStatus: status => status <= 511
    }

    setAccessToken(token: string | null) {
        this._accessToken = token
    }

    setExpiredInSeconds(expiredIn: number) {
        this._expiredAt = DateTime.now().plus({seconds: expiredIn})
    }

    get isAuthenticated() {
        return !!this._accessToken && this._expiredAt > DateTime.now()
    }

    async get<TResponse>(path: string, config: AxiosRequestConfig = {}): Promise<TResponse | null> {
        try {
            const response = await Axios.get<TResponse>(`${this._baseUrl}/${path}`, {
                ...this._globalConfig,
                headers: {
                    Authorization: this._accessToken ? `Bearer ${this._accessToken}` : undefined
                },
                withCredentials: !!this._accessToken,
                ...config
            })

            return response.data
        } catch (e) {
            return null
        }
    }

    async post<TResponse>(path: string, data?: any, config: AxiosRequestConfig = {}): Promise<TResponse | null> {
        try {
            const response = await Axios.post<TResponse>(`${this._baseUrl}/${path}`, data, {
                ...this._globalConfig,
                headers: {
                    Authorization: this._accessToken ? `Bearer ${this._accessToken}` : undefined
                },
                withCredentials: !!this._accessToken,
                ...config
            })

            return response.data
        } catch (e) {
            return null
        }
    }

    async put<TResponse>(path: string, data?: any, config: AxiosRequestConfig = {}): Promise<TResponse | null> {
        try {
            const response = await Axios.put<TResponse>(`${this._baseUrl}/${path}`, data, {
                ...this._globalConfig,
                headers: {
                    Authorization: this._accessToken ? `Bearer ${this._accessToken}` : undefined
                },
                withCredentials: !!this._accessToken,
                ...config
            })

            return response.data
        } catch (e) {
            return null
        }
    }

    async patch<TResponse>(path: string, data?: any, config: AxiosRequestConfig = {}): Promise<TResponse | null> {
        try {
            const response = await Axios.patch<TResponse>(`${this._baseUrl}/${path}`, data, {
                ...this._globalConfig,
                headers: {
                    Authorization: this._accessToken ? `Bearer ${this._accessToken}` : undefined
                },
                withCredentials: !!this._accessToken,
                ...config
            })

            return response.data
        } catch (e) {
            return null
        }
    }

    async delete<TResponse>(path: string, config: AxiosRequestConfig = {}): Promise<TResponse | null> {
        try {
            const response = await Axios.delete<TResponse>(`${this._baseUrl}/${path}`, {
                ...this._globalConfig,
                headers: {
                    Authorization: this._accessToken ? `Bearer ${this._accessToken}` : undefined
                },
                withCredentials: !!this._accessToken,
                ...config
            })

            return response.data
        } catch (e) {
            return null
        }
    }
}

export default Api