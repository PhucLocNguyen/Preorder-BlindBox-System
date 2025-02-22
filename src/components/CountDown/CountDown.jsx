import React, { useEffect, useState } from 'react';

const CountdownTimer = () => {
	const [timeLeft, setTimeLeft] = useState({
		days: 8,
		hours: 3,
		minutes: 51,
		seconds: 21,
	});

	useEffect(() => {
		const timer = setInterval(() => {
			setTimeLeft((prev) => {
				const newSeconds = prev.seconds - 1;
				if (newSeconds >= 0) return { ...prev, seconds: newSeconds };

				const newMinutes = prev.minutes - 1;
				if (newMinutes >= 0) return { ...prev, minutes: newMinutes, seconds: 59 };

				const newHours = prev.hours - 1;
				if (newHours >= 0) return { ...prev, hours: newHours, minutes: 59, seconds: 59 };

				const newDays = prev.days - 1;
				if (newDays >= 0) return { days: newDays, hours: 23, minutes: 59, seconds: 59 };

				clearInterval(timer);
				return prev;
			});
		}, 1000);

		return () => clearInterval(timer);
	}, []);

	// Ensure two-digit formatting
	const formatTime = (num) => String(num).padStart(2, '0');

	return (
		<div className='bg-red-600 text-white rounded-lg p-4 flex items-center justify-center gap-4'>
			<div className='text-center'>
				<div className='text-3xl font-bold'>{formatTime(timeLeft.days)}</div>
				<div className='text-sm'>ngày</div>
			</div>
			<div className='text-3xl font-bold'>:</div>
			<div className='text-center'>
				<div className='text-3xl font-bold'>{formatTime(timeLeft.hours)}</div>
				<div className='text-sm'>giờ</div>
			</div>
			<div className='text-3xl font-bold'>:</div>
			<div className='text-center'>
				<div className='text-3xl font-bold'>{formatTime(timeLeft.minutes)}</div>
				<div className='text-sm'>phút</div>
			</div>
			<div className='text-3xl font-bold'>:</div>
			<div className='text-center'>
				<div className='text-3xl font-bold'>{formatTime(timeLeft.seconds)}</div>
				<div className='text-sm'>giây</div>
			</div>
		</div>
	);
};

export default CountdownTimer;
