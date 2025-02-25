import axios from "axios";
import { GetAccessToken } from "./User/ApiAuthentication";

const api = axios.create({
   baseURL: 'https://preorderblindboxsystem-c9ftb6dtcvdkh3ge.centralus-01.azurewebsites.net/api'
})
api.interceptors.request.use(
   (config) => {
      const accessToken = GetAccessToken();
      if (accessToken) {
         config.headers.Authorization = `Bearer ${accessToken}`;
      }
      return config
   },
   (error) => {
      return Promise.reject(error);
   }
)

export default api;