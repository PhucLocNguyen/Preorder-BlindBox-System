import { createContext, useEffect, useState } from "react";

export const AuthContext = createContext()

const AuthProvider = ({ children }) => {
   const [auth, setAuth] = useState({});
   const [loading, setLoading] = useState(true);

   useEffect(() => {
      const fetchUser = async () => {
         try {

         } catch (error) {
            console.log('Not authenticated');

         } finally {
            setLoading(false)
         }
      }

      fetchUser()
   }, [])

   return (
      <AuthContext.Provider value={{ auth, setAuth }}>
         {!loading && children}
      </AuthContext.Provider>
   )
}
export default AuthProvider