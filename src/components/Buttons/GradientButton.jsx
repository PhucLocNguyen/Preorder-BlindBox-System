import React from 'react';
import { AntDesignOutlined } from '@ant-design/icons';
import { Button, ConfigProvider, Space } from 'antd';
import { createStyles } from 'antd-style';

const useStyle = createStyles(({ token, css, prefixCls }) => ({
  linearGradientButton: css`
    .${prefixCls}-btn-primary {
      position: relative;
      background: linear-gradient(135deg, #6253e1, #04befe);
      border: none;
      color: #fff;

      &:hover {
        background: #04befe;
      }

      &:focus {
        background: #6253e1;
      }

      &::before {
        content: '';
        position: absolute;
        inset: -2px;
        background: linear-gradient(135deg, #6253e1, #04befe);
        border-radius: inherit;
        z-index: -1;
        opacity: 0.7;
        transition: opacity 0.3s ease;
      }

      &:hover::before {
        opacity: 1;
      }
    }
  `,
}));

const GradientButton = ({text, onClick, style, disabled = false}) => {
  const { styles } = useStyle();

  return (
    <ConfigProvider
      theme={{
        components: {
          Button: {
            borderRadius: 8, // Rounded corners for buttons
            colorPrimary: '#6253e1',
          },
        },
      }}
    >
      <div className={styles.linearGradientButton}>
        <Space>
          {/* Gradient Button */}
          <Button type="primary" onClick={onClick} size="large" style={style} disabled={disabled}>
            {text}
          </Button>
        </Space>
      </div>
    </ConfigProvider>
  );
};

export default GradientButton;
