import axios from "axios";

export class Api {
	private static readonly baseUrl: string = process.env.VUE_APP_API_BASEURL;
	protected static readonly axiosInstance = axios.create({
		baseURL: Api.baseUrl,
		withCredentials: true
	})
}
