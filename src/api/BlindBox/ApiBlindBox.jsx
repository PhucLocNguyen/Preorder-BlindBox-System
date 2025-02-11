import api from "../instance";
import { axiosConfigHeader, axiosConfigSendFileHeader } from "../axiosConfigHeader";

const GetTheActiveBlindBox = async (pageSize, pageIndex) => {
  try {
    const response = await api.get("/BlindBox",  {
      ...axiosConfigHeader, // Giữ nguyên cấu hình header
      params: { pageSize, pageIndex }, // Truyền pageSize và pageIndex vào params
    });
    const data = response.data;
    const paginationHeader = response.headers["x-pagination"]; // Chữ thường
    console.log("Pagination Header:", paginationHeader);
    
    const pagination = paginationHeader ? JSON.parse(paginationHeader) : null;

    return {data, pagination};
  } catch (error) {
    console.log(">>> Api Get Active Blind box Error: ", error);
    return { data: [], pagination: null };
  }
};

 const CreateBlindBox = async({ formData: payload})=>{
   try {
      const respone = await api.post('/BlindBox', payload, axiosConfigSendFileHeader);
      return respone.data;
   } catch (error) {
      console.log('>>> Api create blind box Error: ', error)
   }
 }
export { GetTheActiveBlindBox, CreateBlindBox };
