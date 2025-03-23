import { createContext, useEffect, useState } from "react";
import { ApiGetCurrentAccountRole } from "../api/User/ApiAuthentication";
import { ApiGetUserInFormation } from "../api/User/ApiGetUserInformation";

export const AuthContext = createContext()

const AuthProvider = ({ children }) => {
   const [auth, setAuth] = useState({});
   const [loading, setLoading] = useState(true);
   const [currentInformation, setCurrentInformation] = useState({});
   useEffect(() => {
      const fetchUser = async () => {
         try {
            const response = await ApiGetCurrentAccountRole();
            setAuth({
               roleName: response.roleName,
            })
            const getUserInformation = await ApiGetUserInFormation();
            setCurrentInformation(getUserInformation);
         } catch (error) {
            console.log('Not authenticated');
            setAuth({
               roleName: 'Guest'
            })
         } finally {
            setLoading(false)
         }
      }

      fetchUser()
   }, [])

   return (
      <AuthContext.Provider value={{ auth, setAuth , currentInformation, setCurrentInformation}}>
         {!loading && children}
      </AuthContext.Provider>
   )
}
export default AuthProvider