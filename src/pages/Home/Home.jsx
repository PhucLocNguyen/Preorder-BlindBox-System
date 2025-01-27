import { Steps } from "antd";
import CountdownSection from "../../components/Sections/CountdownSection";
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
      <CountdownSection/>
    </div>
  );
}

export default Home;
