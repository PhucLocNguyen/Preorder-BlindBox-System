import { Steps } from "antd";
import Countdown from "../../components/Countdown";
const description = 'This is a description.';
function Home() {
  return (
    <div>
        <p>Hello world</p>
      <Steps
        current={1}
        items={[
          {
            title: "Finished",
            description,
          },
          {
            title: "In Progress",
            description,
            subTitle: "Left 00:00:08",
          },
          {
            title: "Waiting",
            description,
          },
        ]}
      />
      <Countdown/>
    </div>
  );
}

export default Home;
