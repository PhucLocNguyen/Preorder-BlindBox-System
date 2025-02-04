function formatShortVND(amount) {
    if (amount == null) return "0";

    if (amount >= 1_000_000_000) {
        return (amount / 1_000_000_000).toFixed(1).replace(".0", "") + " tỷ";
    } 
    else if (amount >= 1_000_000) {
        return (amount / 1_000_000).toFixed(1).replace(".0", "") + "tr";
    } 
    else if (amount >= 1_000) {
        return (amount / 1_000).toFixed(1).replace(".0", "") + "k";
    } 
    else {
        return amount.toLocaleString('vi-VN') + "đ";
    }
}
export {formatShortVND}