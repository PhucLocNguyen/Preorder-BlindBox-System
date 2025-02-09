import { useParams } from "react-router";

function ProductEdit() {
    const { id } = useParams();
    return ( <div>
        <h2>Product edit {id}</h2>
    </div> );
}

export default ProductEdit;