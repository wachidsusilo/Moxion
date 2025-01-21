import KinematicSimulateRequest from "./requests/KinematicSimulateRequest";
import KinematicSimulateResponse from "./responses/KinematicSimulateResponse";
import Api from "./Api";

class KinematicApi {
    private static readonly _api: Api = new Api('https://localhost/api/v1/kinematic');

    static async simulate(request: KinematicSimulateRequest): Promise<KinematicSimulateResponse | null> {
        return await this._api.post<KinematicSimulateResponse>('simulate', request)
    }
}

export default KinematicApi