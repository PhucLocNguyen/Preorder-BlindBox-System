import api from '../instance.jsx'

const ApiLoginByEmailAndPassword = async ({ payload }) => {
   try {
      const result = await api.post('/Authen/login', payload)
      return result
   } catch (error) {
      console.log('>>> Api login by email and password error: ', error)
   }
}

const ApiGetCurrentAccountRole = async () => {
   try {
      const result = await api.get('/Authen/CurrentAccountRole')
      if (result.status === 200) {
         return result.data
      }
   } catch (error) {
      console.log('>>> Api get current account role error: ', error)
   }
}

export { ApiLoginByEmailAndPassword, ApiGetCurrentAccountRole }